using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Aprendiz.Premium.Commands.PostWaitlist
{
    public sealed record PostWaitlistCommand(int UserId, int PlanId, string PaymentMethod) : IRequest<Result>;
}
