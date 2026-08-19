using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Aprendiz.Premium.Commands.MarkReviewSeen
{
    public sealed record MarkReviewSeenCommand(int UserId) : IRequest<Result>;
}
