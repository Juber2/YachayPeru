using MediatR;
using YachayPeru.Application.Abstractions.Persistence;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Application.Common.Results;
using static YachayPeru.Application.Common.Results.ResultCodes;

namespace YachayPeru.Application.Features.Noticias.Commands.UploadNoticiaImage
{
    public sealed record UploadNoticiaImageCommand : IRequest<Result<string>>
    {
        public int Id { get; init; }
        public Stream FileStream { get; init; } = Stream.Null;
        public string FileName { get; init; } = string.Empty;
    }

    public class UploadNoticiaImageHandler : IRequestHandler<UploadNoticiaImageCommand, Result<string>>
    {
        private readonly INoticiaRepository repository;
        private readonly IFileStorageService fileStorage;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUser currentUser;

        public UploadNoticiaImageHandler(
            INoticiaRepository _repository, IFileStorageService _fileStorage, IUnitOfWork _unitOfWork, ICurrentUser _currentUser)
        {
            repository = _repository;
            fileStorage = _fileStorage;
            unitOfWork = _unitOfWork;
            currentUser = _currentUser;
        }

        public async Task<Result<string>> Handle(UploadNoticiaImageCommand request, CancellationToken ct)
        {
            var noticia = await repository.GetByIdAsync(request.Id, ct);
            if (noticia is null)
                return Result<string>.Failure("Noticia no encontrada.", NotFound);

            var url = await fileStorage.SaveAsync(request.FileStream, request.FileName, "noticias", ct);

            noticia.ImageUrl = url;
            noticia.UpdatedAt = DateTime.UtcNow;
            noticia.UpdatedBy = currentUser.Id;

            repository.Update(noticia);
            await unitOfWork.SaveChangesAsync(ct);

            return Result<string>.Success(url);
        }
    }
}
