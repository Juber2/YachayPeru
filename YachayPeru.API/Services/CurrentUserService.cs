using YachayPeru.Application.Abstractions.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace YachayPeru.API.Services
{
    public sealed class CurrentUserService : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

        public int    Id           => int.TryParse(User?.FindFirstValue(JwtRegisteredClaimNames.Sub), out var id) ? id : 0;
        public string Username     => User?.FindFirstValue(JwtRegisteredClaimNames.UniqueName) ?? string.Empty;
        public string Email        => User?.FindFirstValue("email")        ?? string.Empty;
        public string FullName     => User?.FindFirstValue("fullName")     ?? string.Empty;
        public string UserTypeCode => User?.FindFirstValue("userTypeCode") ?? string.Empty;
    }
}
