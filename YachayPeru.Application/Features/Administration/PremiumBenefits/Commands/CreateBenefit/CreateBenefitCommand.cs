using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Administration.PremiumBenefits.Commands.CreateBenefit
{
    public sealed record CreateBenefitCommand : IRequest<Result<int>>
    {
        public string Code { get; init; } = string.Empty;
        public string Label { get; init; } = string.Empty;
        public string? Description { get; init; }
    }
}
