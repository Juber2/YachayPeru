using MediatR;
using YachayPeru.Application.Abstractions.Persistence;
using YachayPeru.Application.Abstractions.Persistence.Aprendiz;
using YachayPeru.Application.Abstractions.Persistence.Auth;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Application.Common.Results;
using YachayPeru.Domain.Constants;
using static YachayPeru.Application.Common.Results.ResultCodes;

namespace YachayPeru.Application.Features.Administration.Premium.Commands.ReviewWaitlist
{
    public class ReviewWaitlistHandler : IRequestHandler<ReviewWaitlistCommand, Result>
    {
        private readonly IPremiumWaitlistEntryRepository waitlistRepository;
        private readonly IPremiumPlanRepository planRepository;
        private readonly IUserRepository userRepository;
        private readonly IEmailService emailService;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUser currentUser;

        public ReviewWaitlistHandler(
            IPremiumWaitlistEntryRepository _waitlistRepository,
            IPremiumPlanRepository _planRepository,
            IUserRepository _userRepository,
            IEmailService _emailService,
            IUnitOfWork _unitOfWork,
            ICurrentUser _currentUser)
        {
            waitlistRepository = _waitlistRepository;
            planRepository = _planRepository;
            userRepository = _userRepository;
            emailService = _emailService;
            unitOfWork = _unitOfWork;
            currentUser = _currentUser;
        }

        public async Task<Result> Handle(ReviewWaitlistCommand request, CancellationToken ct)
        {
            var isApprove = request.Status == AppConstants.PremiumWaitlistStatusCode.Approved;
            var isReject = request.Status == AppConstants.PremiumWaitlistStatusCode.Rejected;
            if (!isApprove && !isReject)
                return Result.Failure("Estado inválido — debe ser 'approved' o 'rejected'.", Validation);

            var entry = await waitlistRepository.GetByUserIdAsync(request.UserId, ct);
            if (entry is null)
                return Result.Failure("El usuario no está en la waitlist de Premium.", NotFound);

            if (isApprove && string.IsNullOrEmpty(entry.ReceiptUrl))
                return Result.Failure("No se puede aprobar sin comprobante adjunto.", Validation);

            entry.Status = request.Status;
            entry.RejectionReason = isApprove ? null : request.RejectionReason;
            entry.ReviewSeen = false;
            entry.UpdatedAt = DateTime.UtcNow;
            entry.UpdatedBy = currentUser.Id;

            waitlistRepository.Update(entry);
            await unitOfWork.SaveChangesAsync(ct);

            // Aviso por email — no debe revertir la revisión ya guardada si falla.
            var user = await userRepository.GetByIdWithPersonAsync(entry.UserId, ct);
            var plan = await planRepository.GetByIdAsync(entry.PlanId, ct);
            if (user?.Email is not null)
            {
                var fullName = user.Person is null ? string.Empty : $"{user.Person.FirstName} {user.Person.LastName}".Trim();
                try
                {
                    await emailService.SendPremiumReviewEmailAsync(user.Email, fullName, plan?.Name ?? string.Empty, isApprove, entry.RejectionReason, ct);
                }
                catch
                {
                    // El aviso en la app (badge) sigue funcionando aunque el email falle.
                }
            }

            return Result.Success();
        }
    }
}
