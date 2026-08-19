using MediatR;
using YachayPeru.Application.Abstractions.Persistence;
using YachayPeru.Application.Abstractions.Persistence.Aprendiz;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Application.Common.Results;
using static YachayPeru.Application.Common.Results.ResultCodes;

namespace YachayPeru.Application.Features.Aprendiz.Premium.Commands.UploadReceipt
{
    public class UploadReceiptHandler : IRequestHandler<UploadReceiptCommand, Result<string>>
    {
        private readonly IPremiumWaitlistEntryRepository waitlistRepository;
        private readonly IFileStorageService fileStorage;
        private readonly IUnitOfWork unitOfWork;

        public UploadReceiptHandler(
            IPremiumWaitlistEntryRepository _waitlistRepository,
            IFileStorageService _fileStorage,
            IUnitOfWork _unitOfWork)
        {
            waitlistRepository = _waitlistRepository;
            fileStorage = _fileStorage;
            unitOfWork = _unitOfWork;
        }

        public async Task<Result<string>> Handle(UploadReceiptCommand request, CancellationToken ct)
        {
            var entry = await waitlistRepository.GetByUserIdAsync(request.UserId, ct);
            if (entry is null)
                return Result<string>.Failure("Primero tenés que anotarte a un plan.", NotFound);

            var url = await fileStorage.SaveAsync(request.FileStream, request.FileName, "premium-receipts", ct);

            entry.ReceiptUrl = url;
            entry.UpdatedAt = DateTime.UtcNow;
            entry.UpdatedBy = request.UserId;

            waitlistRepository.Update(entry);
            await unitOfWork.SaveChangesAsync(ct);

            return Result<string>.Success(url);
        }
    }
}
