using MediatR;
using YachayPeru.Application.Abstractions.Persistence.Aprendiz;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Domain.Constants;

namespace YachayPeru.Application.Features.Aprendiz.Premium.Queries.GetPlans
{
    public class GetPlansHandler : IRequestHandler<GetPlansQuery, AprendizPremiumPlans>
    {
        private readonly IPremiumPlanRepository planRepository;
        private readonly IPremiumBenefitRepository benefitRepository;
        private readonly IPremiumWaitlistEntryRepository waitlistRepository;

        public GetPlansHandler(
            IPremiumPlanRepository _planRepository,
            IPremiumBenefitRepository _benefitRepository,
            IPremiumWaitlistEntryRepository _waitlistRepository)
        {
            planRepository = _planRepository;
            benefitRepository = _benefitRepository;
            waitlistRepository = _waitlistRepository;
        }

        public async Task<AprendizPremiumPlans> Handle(GetPlansQuery request, CancellationToken ct)
        {
            var plans = await planRepository.ListAsync(p => p.IsActive, ct);
            var benefits = await benefitRepository.ListAsync(ct);
            var labelsById = benefits.ToDictionary(b => b.Id, b => b.Label);

            var cards = new List<AprendizPremiumPlanCard>();
            foreach (var plan in plans.OrderBy(p => p.Price))
            {
                var featureIds = await planRepository.GetFeatureBenefitIdsAsync(plan.Id, ct);
                cards.Add(new AprendizPremiumPlanCard
                {
                    Id = plan.Id,
                    Name = plan.Name,
                    Price = plan.Price,
                    Features = featureIds.Where(labelsById.ContainsKey).Select(id => labelsById[id]).ToList()
                });
            }

            var entry = await waitlistRepository.GetByUserIdAsync(request.UserId, ct);

            return new AprendizPremiumPlans
            {
                IsPremiumUser = entry?.Status == AppConstants.PremiumWaitlistStatusCode.Approved,
                SelectedPlanId = entry?.PlanId,
                WaitlistStatus = entry?.Status,
                RejectionReason = entry?.RejectionReason,
                HasUnseenReview = entry is not null
                    && entry.Status != AppConstants.PremiumWaitlistStatusCode.Pending
                    && !entry.ReviewSeen,
                Plans = cards
            };
        }
    }
}
