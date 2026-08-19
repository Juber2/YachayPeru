using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Administration.PremiumBenefits.Commands.EditBenefit
{
    public sealed record EditBenefitCommand : IRequest<Result>
    {
        public int Id { get; init; }
        public string Code { get; init; } = string.Empty;
        public string Label { get; init; } = string.Empty;
        public string? Description { get; init; }
    }
}
