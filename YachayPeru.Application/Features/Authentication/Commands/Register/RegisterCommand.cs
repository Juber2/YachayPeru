using YachayPeru.Application.Abstractions.Messaging;
using YachayPeru.Application.Common.Results;
using YachayPeru.Application.Features.Authentication.Response;

namespace YachayPeru.Application.Features.Authentication.Commands.Register
{
    public sealed record RegisterCommand : ITransactionalCommand<Result<AuthResult>>
    {
        public string FullName { get; init; } = default!;
        public string Email { get; init; } = default!;
        public string Password { get; init; } = default!;
        public string? IpAddress { get; init; }
        public string? UserAgent { get; init; }
    }
}
