using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Administration.PremiumBenefits.Commands.DeleteBenefit
{
    public sealed record DeleteBenefitCommand(int Id) : IRequest<Result>;
}
