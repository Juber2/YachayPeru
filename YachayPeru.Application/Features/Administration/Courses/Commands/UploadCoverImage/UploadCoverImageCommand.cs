using MediatR;
using YachayPeru.Application.Abstractions.Persistence;
using YachayPeru.Application.Abstractions.Persistence.Learning;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Application.Common.Results;
using static YachayPeru.Application.Common.Results.ResultCodes;

namespace YachayPeru.Application.Features.Administration.Courses.Commands.UploadCoverImage
{
    public sealed record UploadCoverImageCommand : IRequest<Result<string>>
    {
        public int Id { get; init; }
        public Stream FileStream { get; init; } = Stream.Null;
        public string FileName { get; init; } = string.Empty;
    }

    public class UploadCoverImageHandler : IRequestHandler<UploadCoverImageCommand, Result<string>>
    {
        private readonly ICourseRepository repository;
        private readonly IFileStorageService fileStorage;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUser currentUser;

        public UploadCoverImageHandler(
            ICourseRepository _repository, IFileStorageService _fileStorage, IUnitOfWork _unitOfWork, ICurrentUser _currentUser)
        {
            repository = _repository;
            fileStorage = _fileStorage;
            unitOfWork = _unitOfWork;
            currentUser = _currentUser;
        }

        public async Task<Result<string>> Handle(UploadCoverImageCommand request, CancellationToken ct)
        {
            var region = await repository.GetByIdAsync(request.Id, ct);
            if (region is null)
                return Result<string>.Failure("Región no encontrada.", NotFound);

            var url = await fileStorage.SaveAsync(request.FileStream, request.FileName, "courses", ct);

            region.CoverImageUrl = url;
            region.UpdatedAt = DateTime.UtcNow;
            region.UpdatedBy = currentUser.Id;

            repository.Update(region);
            await unitOfWork.SaveChangesAsync(ct);

            return Result<string>.Success(url);
        }
    }
}
