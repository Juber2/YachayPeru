using MediatR;
using YachayPeru.Application.Abstractions.Persistence;
using YachayPeru.Application.Abstractions.Persistence.Learning;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Application.Common.Results;
using YachayPeru.Domain.Entities.Learning;
using static YachayPeru.Application.Common.Results.ResultCodes;

namespace YachayPeru.Application.Features.Courses.Commands.Content
{
    public sealed record UploadItemFileCommand : IRequest<Result<string>>
    {
        public int ItemId { get; init; }
        public string FileTypeCode { get; init; } = string.Empty;
        public Stream FileStream { get; init; } = Stream.Null;
        public string FileName { get; init; } = string.Empty;
    }

    public class UploadItemFileHandler : IRequestHandler<UploadItemFileCommand, Result<string>>
    {
        private readonly IModuleContentRepository contentRepository;
        private readonly IModuleContentFileRepository fileRepository;
        private readonly IFileStorageService fileStorage;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUser currentUser;

        public UploadItemFileHandler(
            IModuleContentRepository _contentRepository,
            IModuleContentFileRepository _fileRepository,
            IFileStorageService _fileStorage,
            IUnitOfWork _unitOfWork,
            ICurrentUser _currentUser)
        {
            contentRepository = _contentRepository;
            fileRepository    = _fileRepository;
            fileStorage       = _fileStorage;
            unitOfWork        = _unitOfWork;
            currentUser       = _currentUser;
        }

        public async Task<Result<string>> Handle(UploadItemFileCommand request, CancellationToken ct)
        {
            var item = await contentRepository.GetByIdAsync(request.ItemId, ct);
            if (item is null)
                return Result<string>.Failure("El ítem de contenido no existe.", NotFound);

            var existingFiles = await fileRepository.GetByItemAsync(request.ItemId, ct);
            var nextOrder = existingFiles.Count > 0 ? existingFiles.Max(f => f.OrderIndex) + 1 : 1;

            var fileUrl = await fileStorage.SaveAsync(request.FileStream, request.FileName, "courses", ct);

            var contentFile = new ModuleContentFile
            {
                ModuleContentId = request.ItemId,
                FileTypeCode    = request.FileTypeCode,
                FileUrl         = fileUrl,
                FileName        = request.FileName,
                OrderIndex      = nextOrder,
                CreatedAt       = DateTime.UtcNow,
                CreatedBy       = currentUser.Id
            };

            await fileRepository.AddAsync(contentFile, ct);
            await unitOfWork.SaveChangesAsync(ct);

            return Result<string>.Success(fileUrl);
        }
    }
}
