using MediatR;
using YachayPeru.Application.Abstractions.Persistence;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Application.Common.Results;
using static YachayPeru.Application.Common.Results.ResultCodes;

namespace YachayPeru.Application.Features.Biblioteca.Commands.UploadMediaItemThumbnail
{
    public sealed record UploadMediaItemThumbnailCommand : IRequest<Result<string>>
    {
        public int Id { get; init; }
        public Stream FileStream { get; init; } = Stream.Null;
        public string FileName { get; init; } = string.Empty;
    }

    public class UploadMediaItemThumbnailHandler : IRequestHandler<UploadMediaItemThumbnailCommand, Result<string>>
    {
        private readonly IMediaItemRepository repository;
        private readonly IFileStorageService fileStorage;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUser currentUser;

        public UploadMediaItemThumbnailHandler(
            IMediaItemRepository _repository, IFileStorageService _fileStorage, IUnitOfWork _unitOfWork, ICurrentUser _currentUser)
        {
            repository = _repository;
            fileStorage = _fileStorage;
            unitOfWork = _unitOfWork;
            currentUser = _currentUser;
        }

        public async Task<Result<string>> Handle(UploadMediaItemThumbnailCommand request, CancellationToken ct)
        {
            var item = await repository.GetByIdAsync(request.Id, ct);
            if (item is null)
                return Result<string>.Failure("Recurso no encontrado.", NotFound);

            var url = await fileStorage.SaveAsync(request.FileStream, request.FileName, "biblioteca", ct);

            item.ThumbnailUrl = url;
            item.UpdatedAt = DateTime.UtcNow;
            item.UpdatedBy = currentUser.Id;

            repository.Update(item);
            await unitOfWork.SaveChangesAsync(ct);

            return Result<string>.Success(url);
        }
    }
}
