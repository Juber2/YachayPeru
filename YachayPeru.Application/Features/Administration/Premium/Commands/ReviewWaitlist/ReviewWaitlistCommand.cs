using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Administration.Premium.Commands.ReviewWaitlist
{
    public sealed record ReviewWaitlistCommand(int UserId, string Status, string? RejectionReason) : IRequest<Result>;
}
