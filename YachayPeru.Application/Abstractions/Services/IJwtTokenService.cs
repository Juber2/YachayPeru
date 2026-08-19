using YachayPeru.Domain.Entities.Auth;

namespace YachayPeru.Application.Abstractions.Services
{
    public interface IJwtTokenService
    {
        string GenerateAccessToken(User user, string userTypeCode, IReadOnlyList<string> roleCodes, IReadOnlyList<string> permissions);
        string GenerateRefreshToken();
        DateTime GetAccessTokenExpiration();
    }
}
