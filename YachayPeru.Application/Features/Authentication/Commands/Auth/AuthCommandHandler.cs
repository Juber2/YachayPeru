using MediatR;
using Microsoft.Extensions.Options;
using YachayPeru.Application.Abstractions.Persistence;
using YachayPeru.Application.Abstractions.Persistence.Auth;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Application.Common.Exceptions;
using YachayPeru.Application.Common.Results;
using YachayPeru.Application.Common.Settings;
using YachayPeru.Application.Features.Authentication.Response;
using YachayPeru.Domain.Entities.Auth;
using DomainRefreshToken = YachayPeru.Domain.Entities.Auth.RefreshToken;

namespace YachayPeru.Application.Features.Authentication.Commands.Auth
{
    public sealed class AuthCommandHandler : IRequestHandler<AuthCommand, Result<AuthResult>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IEmailService _emailService;
        private readonly IAuthTokenIssuer _authTokenIssuer;
        private readonly IUnitOfWork _uow;
        private readonly IOptions<SecuritySettings> _securitySettings;

        public AuthCommandHandler(
            IUserRepository userRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IJwtTokenService jwtTokenService,
            IPasswordHasher passwordHasher,
            IEmailService emailService,
            IAuthTokenIssuer authTokenIssuer,
            IUnitOfWork uow,
            IOptions<SecuritySettings> securitySettings)
        {
            _userRepository         = userRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _jwtTokenService        = jwtTokenService;
            _passwordHasher         = passwordHasher;
            _emailService           = emailService;
            _authTokenIssuer        = authTokenIssuer;
            _uow                    = uow;
            _securitySettings       = securitySettings;
        }

        public async Task<Result<AuthResult>> Handle(AuthCommand command, CancellationToken ct)
        {
            if (command.IsRefresh)
                return await HandleRefresh(command, ct);

            return await HandleLogin(command, ct);
        }

        // ─── Login ────────────────────────────────────────────────────────────────

        private async Task<Result<AuthResult>> HandleLogin(AuthCommand command, CancellationToken ct)
        {
            var user = await _userRepository.GetByUsernameWithDetailsAsync(command.Username!, ct);

            if (user is null)
                throw new UnauthorizedException("AUTH_001", "Credenciales inválidas.");

            if (user.IsLocked)
            {
                if (user.LockedUntil is null || user.LockedUntil > DateTime.UtcNow)
                    throw new UnauthorizedException("AUTH_002", "La cuenta está bloqueada.");

                user.IsLocked     = false;
                user.LockedUntil  = null;
                user.LockedReason = null;
                _userRepository.Update(user);
            }

            if (!_passwordHasher.Verify(command.Password!, user.Password))
                throw new UnauthorizedException("AUTH_001", "Credenciales inválidas.");

            var result = await _authTokenIssuer.IssueAsync(user, command.IpAddress, command.UserAgent, ct);
            return Result<AuthResult>.Success(result);
        }

        // ─── Refresh ──────────────────────────────────────────────────────────────

        private async Task<Result<AuthResult>> HandleRefresh(AuthCommand command, CancellationToken ct)
        {
            var existingToken = await _refreshTokenRepository.GetByTokenAsync(command.RefreshToken!, ct);

            if (existingToken is null)
                throw new UnauthorizedException("TOKEN_INVALID", "Token inválido.");

            if (existingToken.IsRevoked)
                throw new UnauthorizedException("SESSION_REVOKED", "La sesión fue revocada.");

            if (existingToken.ExpiresAt < DateTime.UtcNow)
                throw new UnauthorizedException("SESSION_EXPIRED", "La sesión ha expirado, volvé a iniciar sesión.");

            // Reuso detectado → posible robo, revocar todo
            if (existingToken.ReplacedByToken is not null)
            {
                await _uow.BeginTransactionAsync(ct);
                await _refreshTokenRepository.RevokeAllByUserIdAsync(existingToken.UserId, ct);
                await _uow.SaveChangesAsync(ct);
                await _uow.CommitTransactionAsync(ct);
                throw new UnauthorizedException("SESSION_REVOKED", "Sesión revocada por seguridad. Iniciá sesión nuevamente.");
            }

            if (existingToken.IsPendingApproval)
            {
                if (existingToken.ApprovalExpiresAt < DateTime.UtcNow)
                {
                    _refreshTokenRepository.Revoke(existingToken);
                    await _uow.SaveChangesAsync(ct);
                    throw new UnauthorizedException("SESSION_EXPIRED", "El enlace de aprobación expiró. Iniciá sesión nuevamente.");
                }

                throw new UnauthorizedException("SESSION_PENDING_APPROVAL",
                    "Acceso pendiente de aprobación. Revisá tu email para confirmar o revocar la sesión.");
            }

            var user = await _userRepository.GetByIdAsync(existingToken.UserId, ct);

            if (user is null || user.IsLocked)
                throw new UnauthorizedException("AUTH_002", "La cuenta no está disponible.");

            // Detección de IP sospechosa
            var settings = _securitySettings.Value;
            if (settings.EnableSuspiciousIpDetection
                && existingToken.LoginIp is not null
                && !string.IsNullOrEmpty(command.IpAddress)
                && existingToken.LoginIp != command.IpAddress)
            {
                var approvalToken = Guid.NewGuid().ToString("N");

                existingToken.IsPendingApproval = true;
                existingToken.ApprovalToken     = approvalToken;
                existingToken.ApprovalExpiresAt = DateTime.UtcNow.AddHours(settings.ApprovalTokenExpirationHours);
                existingToken.SuspiciousIp      = command.IpAddress;
                _refreshTokenRepository.Update(existingToken);
                await _uow.SaveChangesAsync(ct);

                var approveUrl = $"{settings.BaseUrl}/auth/session/approve?token={approvalToken}";
                var revokeUrl  = $"{settings.BaseUrl}/auth/session/revoke?token={approvalToken}";

                await _emailService.SendSuspiciousSessionAlertAsync(
                    user.Email ?? user.Username, user.Username,
                    command.IpAddress, approveUrl, revokeUrl, ct);

                throw new UnauthorizedException("SESSION_PENDING_APPROVAL",
                    "Acceso desde una ubicación desconocida. Revisá tu email para confirmar o revocar la sesión.");
            }

            var (roleCodes, permissions) = await LoadAccess(user.Id, ct);

            await _uow.BeginTransactionAsync(ct);

            var newAccessToken       = _jwtTokenService.GenerateAccessToken(user, user.UserTypeCode, roleCodes, permissions);
            var newRefreshTokenValue = _jwtTokenService.GenerateRefreshToken();

            existingToken.ReplacedByToken = newRefreshTokenValue;
            _refreshTokenRepository.Update(existingToken);

            await _refreshTokenRepository.AddAsync(new DomainRefreshToken
            {
                UserId         = user.Id,
                Token          = newRefreshTokenValue,
                ExpiresAt      = DateTime.UtcNow.AddDays(7),
                IsRevoked      = false,
                LoginIp        = command.IpAddress,
                LoginUserAgent = command.UserAgent,
                CreatedAt      = DateTime.UtcNow
            }, ct);

            await _uow.SaveChangesAsync(ct);
            await _uow.CommitTransactionAsync(ct);

            var fullName = user.Person is null
                ? "sin datos"
                : $"{user.Person.FirstName} {user.Person.LastName}".Trim();

            return Result<AuthResult>.Success(new AuthResult
            {
                AccessToken          = newAccessToken,
                RefreshToken         = newRefreshTokenValue,
                AccessTokenExpiresAt = _jwtTokenService.GetAccessTokenExpiration(),
                UserId               = user.Id,
                Username             = user.Username,
                FullName             = fullName,
                Email                = user.Email,
                UserTypeCode         = user.UserTypeCode,
                Roles                = roleCodes,
                Permissions          = permissions
            });
        }

        // ─── Shared ───────────────────────────────────────────────────────────────

        private async Task<(List<string> roleCodes, List<string> permissions)>
            LoadAccess(int userId, CancellationToken ct)
        {
            var access = await _userRepository.GetUserAccessAsync(userId, ct);

            var roleCodes = access.Select(r => r.RoleCode).ToList();

            var permissions = access
                .SelectMany(r => r.Permissions)
                .Where(p => !string.IsNullOrEmpty(p.Resource) && !string.IsNullOrEmpty(p.Action))
                .Select(p => $"{p.Resource}:{p.Action}")
                .Distinct()
                .ToList();

            return (roleCodes, permissions);
        }
    }
}
