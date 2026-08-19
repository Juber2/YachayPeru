using YachayPeru.Application.Abstractions.Messaging;
using YachayPeru.Application.Common.Results;
using YachayPeru.Application.Features.Authentication.Response;

namespace YachayPeru.Application.Features.Authentication.Commands.ApproveSession
{
    public sealed record ApproveSessionCommand : ITransactionalCommand<Result<AuthResult>>
    {
        public string ApprovalToken { get; init; } = default!;
    }
}
