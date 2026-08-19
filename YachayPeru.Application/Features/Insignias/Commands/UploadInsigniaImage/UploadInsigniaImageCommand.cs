using MediatR;
using YachayPeru.Application.Abstractions.Persistence;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Application.Common.Results;
using static YachayPeru.Application.Common.Results.ResultCodes;

namespace YachayPeru.Application.Features.Insignias.Commands.UploadInsigniaImage
{
    public sealed record UploadInsigniaImageCommand : IRequest<Result<string>>
    {
        public int Id { get; init; }
        public Stream FileStream { get; init; } = Stream.Null;
        public string FileName { get; init; } = string.Empty;
    }

    public class UploadInsigniaImageHandler : IRequestHandler<UploadInsigniaImageCommand, Result<string>>
    {
        private readonly IInsigniaRepository repository;
        private readonly IFileStorageService fileStorage;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUser currentUser;

        public UploadInsigniaImageHandler(
            IInsigniaRepository _repository, IFileStorageService _fileStorage, IUnitOfWork _unitOfWork, ICurrentUser _currentUser)
        {
            repository = _repository;
            fileStorage = _fileStorage;
            unitOfWork = _unitOfWork;
            currentUser = _currentUser;
        }

        public async Task<Result<string>> Handle(UploadInsigniaImageCommand request, CancellationToken ct)
        {
            var insignia = await repository.GetByIdAsync(request.Id, ct);
            if (insignia is null)
                return Result<string>.Failure("Insignia no encontrada.", NotFound);

            var url = await fileStorage.SaveAsync(request.FileStream, request.FileName, "insignias", ct);

            insignia.ImageUrl = url;
            insignia.UpdatedAt = DateTime.UtcNow;
            insignia.UpdatedBy = currentUser.Id;

            repository.Update(insignia);
            await unitOfWork.SaveChangesAsync(ct);

            return Result<string>.Success(url);
        }
    }
}
