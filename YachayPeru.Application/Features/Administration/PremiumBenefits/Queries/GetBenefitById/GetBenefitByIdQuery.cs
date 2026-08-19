using MediatR;
using YachayPeru.Application.Common.Results;
using YachayPeru.Application.Features.Administration.PremiumBenefits.Queries.GetBenefits;

namespace YachayPeru.Application.Features.Administration.PremiumBenefits.Queries.GetBenefitById
{
    public sealed record GetBenefitByIdQuery(int Id) : IRequest<Result<PremiumBenefitDto>>;
}
