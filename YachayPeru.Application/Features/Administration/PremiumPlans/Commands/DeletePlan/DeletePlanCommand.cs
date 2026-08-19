using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Administration.PremiumPlans.Commands.DeletePlan
{
    public sealed record DeletePlanCommand(int Id) : IRequest<Result>;
}
