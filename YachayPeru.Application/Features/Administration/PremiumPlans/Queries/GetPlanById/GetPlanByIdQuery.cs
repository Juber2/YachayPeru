using MediatR;
using YachayPeru.Application.Common.Results;
using YachayPeru.Application.Features.Administration.PremiumPlans.Queries.GetPlans;

namespace YachayPeru.Application.Features.Administration.PremiumPlans.Queries.GetPlanById
{
    public sealed record GetPlanByIdQuery(int Id) : IRequest<Result<PremiumPlanDto>>;
}
