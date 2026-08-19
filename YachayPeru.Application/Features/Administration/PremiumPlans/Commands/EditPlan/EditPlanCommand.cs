using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Administration.PremiumPlans.Commands.EditPlan
{
    public sealed record EditPlanCommand : IRequest<Result>
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public decimal Price { get; init; }
        public bool IsActive { get; init; } = true;
        public List<int> FeatureBenefitIds { get; init; } = new();
    }
}
