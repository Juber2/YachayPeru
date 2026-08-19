using MediatR;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Application.Common.Results;
using YachayPeru.Application.Features.Administration.PremiumBenefits.Queries.GetBenefits;

namespace YachayPeru.Application.Features.Administration.PremiumPlans.Queries.GetPlans
{
    public class GetPlansHandler : IRequestHandler<GetPlansQuery, Result<List<PremiumPlanDto>>>
    {
        private readonly IPremiumPlanRepository planRepository;
        private readonly IPremiumBenefitRepository benefitRepository;

        public GetPlansHandler(IPremiumPlanRepository _planRepository, IPremiumBenefitRepository _benefitRepository)
        {
            planRepository = _planRepository;
            benefitRepository = _benefitRepository;
        }

        public async Task<Result<List<PremiumPlanDto>>> Handle(GetPlansQuery request, CancellationToken ct)
        {
            var plans = await planRepository.ListAsync(ct);
            var benefits = await benefitRepository.ListAsync(ct);
            var benefitsById = benefits.ToDictionary(b => b.Id, b => new PremiumBenefitDto(b.Id, b.Code, b.Label, b.Description));

            var result = new List<PremiumPlanDto>();
            foreach (var plan in plans.OrderBy(p => p.Price))
            {
                var featureIds = await planRepository.GetFeatureBenefitIdsAsync(plan.Id, ct);
                result.Add(new PremiumPlanDto
                {
                    Id = plan.Id,
                    Name = plan.Name,
                    Price = plan.Price,
                    IsActive = plan.IsActive,
                    Features = featureIds.Where(benefitsById.ContainsKey).Select(id => benefitsById[id]).ToList()
                });
            }

            return Result<List<PremiumPlanDto>>.Success(result);
        }
    }
}
