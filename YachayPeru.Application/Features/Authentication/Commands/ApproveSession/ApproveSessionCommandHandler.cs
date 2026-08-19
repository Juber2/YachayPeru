using MediatR;
using YachayPeru.Application.Abstractions.Persistence;
using YachayPeru.Application.Abstractions.Persistence.Auth;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Application.Common.Exceptions;
using YachayPeru.Application.Common.Results;
using YachayPeru.Application.Features.Authentication.Response;
using DomainRefreshToken = YachayPeru.Domain.Entities.Auth.RefreshToken;

namespace YachayPeru.Application.Features.Authentication.Commands.ApproveSession
{
    public sealed class ApproveSessionCommandHandler : IRequestHandler<ApproveSessionCommand, Result<AuthResult>>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IUnitOfWork _uow;

        public ApproveSessionCommandHandler(
            IRefreshTokenRepository refreshTokenRepository,
            IUserRepository userRepository,
            IJwtTokenService jwtTokenService,
            IUnitOfWork uow)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _userRepository         = userRepository;
            _jwtTokenService        = jwtTokenService;
            _uow                    = uow;
        }

        public async Task<Result<AuthResult>> Handle(ApproveSessionCommand command, CancellationToken ct)
        {
            var existingToken = await _refreshTokenRepository.GetByApprovalTokenAsync(command.ApprovalToken, ct);

            if (existingToken is null || !existingToken.IsPendingApproval || existingToken.IsRevoked)
                throw new UnauthorizedException("TOKEN_INVALID", "Token de aprobación inválido.");

            if (existingToken.ApprovalExpiresAt < DateTime.UtcNow)
                throw new UnauthorizedException("SESSION_EXPIRED",
                    "El enlace de aprobación ha expirado. Iniciá sesión nuevamente.");

            // Carga User + UserType para regenerar el token con claims actualizados
            var user = await _userRepository.GetByIdAsync(existingToken.UserId, ct);

            if (user is null || user.IsLocked)
                throw new UnauthorizedException("AUTH_002", "La cuenta no está disponible.");

            var access      = await _userRepository.GetUserAccessAsync(user.Id, ct);
            var roleCodes   = access.Select(r => r.RoleCode).ToList();
            var permissions = access
                .SelectMany(r => r.Permissions)
                .Where(p => !string.IsNullOrEmpty(p.Resource) && !string.IsNullOrEmpty(p.Action))
                .Select(p => $"{p.Resource}:{p.Action}")
                .Distinct()
                .ToList();

            await _uow.BeginTransactionAsync(ct);

            var newAccessToken       = _jwtTokenService.GenerateAccessToken(user, user.UserTypeCode, roleCodes, permissions);
            var newRefreshTokenValue = _jwtTokenService.GenerateRefreshToken();

            // Marcar token anterior como consumido y aprobado
            existingToken.ReplacedByToken   = newRefreshTokenValue;
            existingToken.IsPendingApproval = false;
            _refreshTokenRepository.Update(existingToken);

            // El nuevo token usa la IP sospechosa aprobada como nueva línea base
            var newRefreshToken = new DomainRefreshToken
            {
                UserId         = user.Id,
                Token          = newRefreshTokenValue,
                ExpiresAt      = DateTime.UtcNow.AddDays(7),
                IsRevoked      = false,
                LoginIp        = existingToken.SuspiciousIp,
                LoginUserAgent = existingToken.LoginUserAgent,
                CreatedAt      = DateTime.UtcNow
            };

            await _refreshTokenRepository.AddAsync(newRefreshToken, ct);
            await _uow.SaveChangesAsync(ct);
            await _uow.CommitTransactionAsync(ct);

            return Result<AuthResult>.Success(new AuthResult
            {
                AccessToken          = newAccessToken,
                RefreshToken         = newRefreshTokenValue,
                AccessTokenExpiresAt = _jwtTokenService.GetAccessTokenExpiration()
            });
        }
    }
}
