using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Administration.PremiumPlans.Commands.CreatePlan
{
    public sealed record CreatePlanCommand : IRequest<Result<int>>
    {
        public string Name { get; init; } = string.Empty;
        public decimal Price { get; init; }
        public bool IsActive { get; init; } = true;
        public List<int> FeatureBenefitIds { get; init; } = new();
    }
}
