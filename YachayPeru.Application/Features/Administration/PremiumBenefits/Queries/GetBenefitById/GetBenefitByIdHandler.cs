using MediatR;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Application.Common.Results;
using YachayPeru.Application.Features.Administration.PremiumBenefits.Queries.GetBenefits;
using static YachayPeru.Application.Common.Results.ResultCodes;

namespace YachayPeru.Application.Features.Administration.PremiumBenefits.Queries.GetBenefitById
{
    public class GetBenefitByIdHandler : IRequestHandler<GetBenefitByIdQuery, Result<PremiumBenefitDto>>
    {
        private readonly IPremiumBenefitRepository benefitRepository;

        public GetBenefitByIdHandler(IPremiumBenefitRepository _benefitRepository) => benefitRepository = _benefitRepository;

        public async Task<Result<PremiumBenefitDto>> Handle(GetBenefitByIdQuery request, CancellationToken ct)
        {
            var benefit = await benefitRepository.GetByIdAsync(request.Id, ct);
            if (benefit is null)
                return Result<PremiumBenefitDto>.Failure("Beneficio no encontrado.", NotFound);

            return Result<PremiumBenefitDto>.Success(new PremiumBenefitDto(benefit.Id, benefit.Code, benefit.Label, benefit.Description));
        }
    }
}
