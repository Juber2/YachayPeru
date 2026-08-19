using YachayPeru.Application.Features.Authentication.Response;
using YachayPeru.Domain.Entities.Auth;

namespace YachayPeru.Application.Abstractions.Services
{
    public interface IAuthTokenIssuer
    {
        Task<AuthResult> IssueAsync(User user, string? ipAddress, string? userAgent, CancellationToken ct = default);
    }
}
