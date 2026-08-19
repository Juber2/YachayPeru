using MediatR;
using YachayPeru.Application.Abstractions.Persistence;
using YachayPeru.Application.Abstractions.Persistence.Aprendiz;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Application.Common.Results;
using YachayPeru.Domain.Constants;
using YachayPeru.Domain.Entities.Aprendiz;
using static YachayPeru.Application.Common.Results.ResultCodes;

namespace YachayPeru.Application.Features.Aprendiz.Premium.Commands.PostWaitlist
{
    public class PostWaitlistHandler : IRequestHandler<PostWaitlistCommand, Result>
    {
        private readonly IPremiumWaitlistEntryRepository waitlistRepository;
        private readonly IPremiumPlanRepository planRepository;
        private readonly IUnitOfWork unitOfWork;

        public PostWaitlistHandler(
            IPremiumWaitlistEntryRepository _waitlistRepository,
            IPremiumPlanRepository _planRepository,
            IUnitOfWork _unitOfWork)
        {
            waitlistRepository = _waitlistRepository;
            planRepository = _planRepository;
            unitOfWork = _unitOfWork;
        }

        public async Task<Result> Handle(PostWaitlistCommand request, CancellationToken ct)
        {
            if (!AppConstants.PaymentMethodCode.All.Contains(request.PaymentMethod))
                return Result.Failure("Método de pago inválido.", Validation);

            var plan = await planRepository.GetByIdAsync(request.PlanId, ct);
            if (plan is null || !plan.IsActive)
                return Result.Failure("El plan elegido no existe o no está disponible.", NotFound);

            var existing = await waitlistRepository.GetByUserIdAsync(request.UserId, ct);
            if (existing is not null)
            {
                // Reinscripción (incluso tras un rechazo): se conserva JoinedAt/ReceiptUrl, se actualiza
                // el plan y el método de pago elegidos, y vuelve a quedar pendiente de revisión.
                existing.PlanId = request.PlanId;
                existing.PaymentMethod = request.PaymentMethod;
                existing.Status = AppConstants.PremiumWaitlistStatusCode.Pending;
                existing.RejectionReason = null;
                existing.UpdatedAt = DateTime.UtcNow;
                existing.UpdatedBy = request.UserId;
                waitlistRepository.Update(existing);
            }
            else
            {
                await waitlistRepository.AddAsync(new PremiumWaitlistEntry
                {
                    UserId = request.UserId,
                    PlanId = request.PlanId,
                    PaymentMethod = request.PaymentMethod,
                    Status = AppConstants.PremiumWaitlistStatusCode.Pending,
                    JoinedAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = request.UserId
                }, ct);
            }

            await unitOfWork.SaveChangesAsync(ct);
            return Result.Success();
        }
    }
}
