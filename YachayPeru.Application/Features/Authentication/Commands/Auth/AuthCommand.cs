using YachayPeru.Application.Abstractions.Messaging;
using YachayPeru.Application.Common.Results;
using YachayPeru.Application.Features.Authentication.Response;

namespace YachayPeru.Application.Features.Authentication.Commands.Auth
{
    public sealed record AuthCommand : ITransactionalCommand<Result<AuthResult>>
    {
        // Login
        public string? Username { get; init; }
        public string? Password { get; init; }

        // Refresh
        public string? RefreshToken { get; init; }

        // Compartidos
        public string? IpAddress { get; init; }
        public string? UserAgent { get; init; }

        public bool IsRefresh => RefreshToken is not null;
    }
}
