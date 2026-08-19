using MediatR;

namespace YachayPeru.Application.Features.Administration.PremiumBenefits.Queries.GetBenefits
{
    public sealed record GetBenefitsQuery : IRequest<List<PremiumBenefitDto>>;

    public sealed record PremiumBenefitDto(int Id, string Code, string Label, string? Description);
}
