using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Domain.Entities.Auth;
using YachayPeru.Infrastructure.Config;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace YachayPeru.Infrastructure.Services
{
    public sealed class JwtTokenService : IJwtTokenService
    {
        private readonly JwtSettings _settings;

        public JwtTokenService(IOptions<JwtSettings> settings)
        {
            _settings = settings.Value;
        }

        public string GenerateAccessToken(User user, string userTypeCode, IReadOnlyList<string> roleCodes, IReadOnlyList<string> permissions)
        {
            var key         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub,        user.Id.ToString()),
                new(JwtRegisteredClaimNames.UniqueName, user.Username),
                new(JwtRegisteredClaimNames.Jti,        Guid.NewGuid().ToString()),
                new("userTypeCode",                     userTypeCode),
                new("email",                     user.Email??""),
                new("fullName",                     user.Person==null?"sin nombre":user.Person.FirstName+user.Person.LastName)
            };

            foreach (var roleCode in roleCodes)
                claims.Add(new Claim("roles", roleCode));

            foreach (var permission in permissions)
                claims.Add(new Claim("permission", permission));

            var token = new JwtSecurityToken(
                issuer:            _settings.Issuer,
                audience:          _settings.Audience,
                claims:            claims,
                expires:           GetAccessTokenExpiration(),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
            => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        public DateTime GetAccessTokenExpiration()
            => DateTime.UtcNow.AddMinutes(_settings.AccessTokenExpirationMinutes);
    }
}
