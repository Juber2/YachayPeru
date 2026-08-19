using MediatR;
using YachayPeru.Application.Common.Results;
using YachayPeru.Application.Features.Administration.PremiumBenefits.Queries.GetBenefits;

namespace YachayPeru.Application.Features.Administration.PremiumPlans.Queries.GetPlans
{
    public sealed record GetPlansQuery : IRequest<Result<List<PremiumPlanDto>>>;

    public sealed record PremiumPlanDto
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public decimal Price { get; init; }
        public bool IsActive { get; init; }
        public List<PremiumBenefitDto> Features { get; init; } = new();
    }
}
