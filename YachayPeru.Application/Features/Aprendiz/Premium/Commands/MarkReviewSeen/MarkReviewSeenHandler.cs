using MediatR;
using YachayPeru.Application.Abstractions.Persistence;
using YachayPeru.Application.Abstractions.Persistence.Aprendiz;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Aprendiz.Premium.Commands.MarkReviewSeen
{
    public class MarkReviewSeenHandler : IRequestHandler<MarkReviewSeenCommand, Result>
    {
        private readonly IPremiumWaitlistEntryRepository waitlistRepository;
        private readonly IUnitOfWork unitOfWork;

        public MarkReviewSeenHandler(IPremiumWaitlistEntryRepository _waitlistRepository, IUnitOfWork _unitOfWork)
        {
            waitlistRepository = _waitlistRepository;
            unitOfWork = _unitOfWork;
        }

        public async Task<Result> Handle(MarkReviewSeenCommand request, CancellationToken ct)
        {
            var entry = await waitlistRepository.GetByUserIdAsync(request.UserId, ct);
            if (entry is null || entry.ReviewSeen)
                return Result.Success(); // idempotente: nada que marcar

            entry.ReviewSeen = true;
            entry.UpdatedAt = DateTime.UtcNow;
            entry.UpdatedBy = request.UserId;

            waitlistRepository.Update(entry);
            await unitOfWork.SaveChangesAsync(ct);

            return Result.Success();
        }
    }
}
