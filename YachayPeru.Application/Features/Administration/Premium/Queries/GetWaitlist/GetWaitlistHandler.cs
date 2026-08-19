using MediatR;
using YachayPeru.Application.Abstractions.Persistence.Aprendiz;
using YachayPeru.Application.Abstractions.Persistence.Auth;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Administration.Premium.Queries.GetWaitlist
{
    public class GetWaitlistHandler : IRequestHandler<GetWaitlistQuery, Result<List<PremiumWaitlistEntryDto>>>
    {
        private readonly IPremiumWaitlistEntryRepository waitlistRepository;
        private readonly IUserRepository userRepository;
        private readonly IPremiumPlanRepository planRepository;

        public GetWaitlistHandler(
            IPremiumWaitlistEntryRepository _waitlistRepository,
            IUserRepository _userRepository,
            IPremiumPlanRepository _planRepository)
        {
            waitlistRepository = _waitlistRepository;
            userRepository = _userRepository;
            planRepository = _planRepository;
        }

        public async Task<Result<List<PremiumWaitlistEntryDto>>> Handle(GetWaitlistQuery request, CancellationToken ct)
        {
            var entries = await waitlistRepository.ListAsync(ct);

            var result = new List<PremiumWaitlistEntryDto>();
            foreach (var entry in entries.OrderByDescending(e => e.JoinedAt))
            {
                var user = await userRepository.GetByIdWithPersonAsync(entry.UserId, ct);
                var fullName = user?.Person is null ? string.Empty : $"{user.Person.FirstName} {user.Person.LastName}".Trim();
                var plan = await planRepository.GetByIdAsync(entry.PlanId, ct);

                result.Add(new PremiumWaitlistEntryDto(
                    entry.Id,
                    entry.UserId,
                    fullName,
                    user?.Email ?? string.Empty,
                    entry.PlanId,
                    plan?.Name ?? string.Empty,
                    entry.PaymentMethod,
                    entry.ReceiptUrl,
                    entry.Status,
                    entry.RejectionReason,
                    entry.JoinedAt));
            }

            return Result<List<PremiumWaitlistEntryDto>>.Success(result);
        }
    }
}
