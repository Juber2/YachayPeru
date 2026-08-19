using YachayPeru.Application.Abstractions.Messaging;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Authentication.Commands.RevokeSession
{
    public sealed record RevokeSessionCommand : ITransactionalCommand<Result<bool>>
    {
        public string ApprovalToken { get; init; } = default!;
    }
}
