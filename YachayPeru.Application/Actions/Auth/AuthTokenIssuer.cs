using YachayPeru.Application.Abstractions.Persistence;
using YachayPeru.Application.Abstractions.Persistence.Aprendiz;
using YachayPeru.Application.Abstractions.Persistence.Auth;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Application.Features.Authentication.Response;
using YachayPeru.Domain.Entities.Auth;

namespace YachayPeru.Application.Actions.Auth
{
    public class AuthTokenIssuer : IAuthTokenIssuer
    {
        private readonly IUserRepository userRepository;
        private readonly IRefreshTokenRepository refreshTokenRepository;
        private readonly IJwtTokenService jwtTokenService;
        private readonly IAprendizProfileRepository profileRepository;
        private readonly IInsigniaEvaluator insigniaEvaluator;
        private readonly IUnitOfWork unitOfWork;

        public AuthTokenIssuer(
            IUserRepository _userRepository,
            IRefreshTokenRepository _refreshTokenRepository,
            IJwtTokenService _jwtTokenService,
            IAprendizProfileRepository _profileRepository,
            IInsigniaEvaluator _insigniaEvaluator,
            IUnitOfWork _unitOfWork)
        {
            userRepository = _userRepository;
            refreshTokenRepository = _refreshTokenRepository;
            jwtTokenService = _jwtTokenService;
            profileRepository = _profileRepository;
            insigniaEvaluator = _insigniaEvaluator;
            unitOfWork = _unitOfWork;
        }

        public async Task<AuthResult> IssueAsync(User user, string? ipAddress, string? userAgent, CancellationToken ct = default)
        {
            var access = await userRepository.GetUserAccessAsync(user.Id, ct);
            var roleCodes = access.Select(r => r.RoleCode).ToList();
            var permissions = access
                .SelectMany(r => r.Permissions)
                .Where(p => !string.IsNullOrEmpty(p.Resource) && !string.IsNullOrEmpty(p.Action))
                .Select(p => $"{p.Resource}:{p.Action}")
                .Distinct()
                .ToList();

            await unitOfWork.BeginTransactionAsync(ct);

            await refreshTokenRepository.RevokeAllByUserIdAsync(user.Id, ct);

            var accessToken = jwtTokenService.GenerateAccessToken(user, user.UserTypeCode, roleCodes, permissions);
            var refreshTokenValue = jwtTokenService.GenerateRefreshToken();

            await refreshTokenRepository.AddAsync(new RefreshToken
            {
                UserId = user.Id,
                Token = refreshTokenValue,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false,
                LoginIp = ipAddress,
                LoginUserAgent = userAgent,
                CreatedAt = DateTime.UtcNow
            }, ct);

            await unitOfWork.SaveChangesAsync(ct);
            await unitOfWork.CommitTransactionAsync(ct);

            // Racha de login del aprendiz — solo se actualiza acá (login real con usuario/contraseña),
            // nunca en un refresh silencioso de token (ese flujo arma el token aparte, sin pasar por
            // IssueAsync). Cambio aislado en su propio SaveChanges, no forma parte de la transacción
            // de emisión de tokens.
            if (roleCodes.Contains("APRENDIZ"))
            {
                var today = DateOnly.FromDateTime(DateTime.UtcNow);
                var profile = await profileRepository.GetByUserIdAsync(user.Id, ct);
                if (profile is null)
                {
                    profile = new Domain.Entities.Aprendiz.AprendizProfile
                    {
                        UserId = user.Id,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = user.Id
                    };
                    await profileRepository.AddAsync(profile, ct);
                }

                if (profile.LastActiveDate != today)
                {
                    profile.CurrentLoginStreakDays = profile.LastActiveDate == today.AddDays(-1)
                        ? profile.CurrentLoginStreakDays + 1
                        : 1;
                    profile.BestLoginStreakDays = Math.Max(profile.BestLoginStreakDays, profile.CurrentLoginStreakDays);
                    profile.LastActiveDate = today;
                    profile.UpdatedAt = DateTime.UtcNow;
                    profile.UpdatedBy = user.Id;
                    profileRepository.Update(profile);

                    await unitOfWork.SaveChangesAsync(ct);
                    await insigniaEvaluator.EvaluateAsync(user.Id, ct);
                }
            }

            var fullName = user.Person is null
                ? string.Empty
                : $"{user.Person.FirstName} {user.Person.LastName}".Trim();

            return new AuthResult
            {
                AccessToken = accessToken,
                RefreshToken = refreshTokenValue,
                AccessTokenExpiresAt = jwtTokenService.GetAccessTokenExpiration(),
                UserId = user.Id,
                Username = user.Username,
                FullName = fullName,
                Email = user.Email,
                UserTypeCode = user.UserTypeCode,
                Roles = roleCodes,
                Permissions = permissions
            };
        }
    }
}
