using MediatR;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Application.Common.Results;
using YachayPeru.Application.Features.Administration.PremiumBenefits.Queries.GetBenefits;
using YachayPeru.Application.Features.Administration.PremiumPlans.Queries.GetPlans;
using static YachayPeru.Application.Common.Results.ResultCodes;

namespace YachayPeru.Application.Features.Administration.PremiumPlans.Queries.GetPlanById
{
    public class GetPlanByIdHandler : IRequestHandler<GetPlanByIdQuery, Result<PremiumPlanDto>>
    {
        private readonly IPremiumPlanRepository planRepository;
        private readonly IPremiumBenefitRepository benefitRepository;

        public GetPlanByIdHandler(IPremiumPlanRepository _planRepository, IPremiumBenefitRepository _benefitRepository)
        {
            planRepository = _planRepository;
            benefitRepository = _benefitRepository;
        }

        public async Task<Result<PremiumPlanDto>> Handle(GetPlanByIdQuery request, CancellationToken ct)
        {
            var plan = await planRepository.GetByIdAsync(request.Id, ct);
            if (plan is null)
                return Result<PremiumPlanDto>.Failure("Plan no encontrado.", NotFound);

            var featureIds = await planRepository.GetFeatureBenefitIdsAsync(plan.Id, ct);
            var features = new List<PremiumBenefitDto>();
            foreach (var benefitId in featureIds)
            {
                var benefit = await benefitRepository.GetByIdAsync(benefitId, ct);
                if (benefit is not null)
                    features.Add(new PremiumBenefitDto(benefit.Id, benefit.Code, benefit.Label, benefit.Description));
            }

            return Result<PremiumPlanDto>.Success(new PremiumPlanDto
            {
                Id = plan.Id,
                Name = plan.Name,
                Price = plan.Price,
                IsActive = plan.IsActive,
                Features = features
            });
        }
    }
}
