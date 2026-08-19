using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Administration.Premium.Queries.GetWaitlist
{
    public sealed record GetWaitlistQuery : IRequest<Result<List<PremiumWaitlistEntryDto>>>;

    public sealed record PremiumWaitlistEntryDto(
        int Id,
        int UserId,
        string UserFullName,
        string UserEmail,
        int PlanId,
        string PlanName,
        string PaymentMethod,
        string? ReceiptUrl,
        string Status,
        string? RejectionReason,
        DateTime JoinedAt);
}
