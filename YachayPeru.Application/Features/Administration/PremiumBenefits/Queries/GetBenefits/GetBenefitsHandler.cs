using MediatR;
using YachayPeru.Application.Abstractions.Persistence.Content;

namespace YachayPeru.Application.Features.Administration.PremiumBenefits.Queries.GetBenefits
{
    public class GetBenefitsHandler : IRequestHandler<GetBenefitsQuery, List<PremiumBenefitDto>>
    {
        private readonly IPremiumBenefitRepository benefitRepository;

        public GetBenefitsHandler(IPremiumBenefitRepository _benefitRepository) => benefitRepository = _benefitRepository;

        public async Task<List<PremiumBenefitDto>> Handle(GetBenefitsQuery request, CancellationToken ct)
        {
            var benefits = await benefitRepository.ListAsync(ct);
            return benefits
                .OrderBy(b => b.Label)
                .Select(b => new PremiumBenefitDto(b.Id, b.Code, b.Label, b.Description))
                .ToList();
        }
    }
}
