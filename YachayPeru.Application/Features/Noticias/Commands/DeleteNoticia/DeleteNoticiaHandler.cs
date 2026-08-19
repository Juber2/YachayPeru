using MediatR;
using YachayPeru.Application.Abstractions.Persistence;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Application.Common.Results;
using static YachayPeru.Application.Common.Results.ResultCodes;

namespace YachayPeru.Application.Features.Noticias.Commands.DeleteNoticia
{
    public class DeleteNoticiaHandler : IRequestHandler<DeleteNoticiaCommand, Result>
    {
        private readonly INoticiaRepository repository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUser currentUser;

        public DeleteNoticiaHandler(INoticiaRepository _repository, IUnitOfWork _unitOfWork, ICurrentUser _currentUser)
        {
            repository = _repository;
            unitOfWork = _unitOfWork;
            currentUser = _currentUser;
        }

        public async Task<Result> Handle(DeleteNoticiaCommand request, CancellationToken ct)
        {
            var noticia = await repository.GetByIdAsync(request.Id, ct);
            if (noticia is null)
                return Result.Failure("Noticia no encontrada.", NotFound);

            noticia.Deleted = true;
            noticia.UpdatedAt = DateTime.UtcNow;
            noticia.UpdatedBy = currentUser.Id;

            repository.Update(noticia);
            await unitOfWork.SaveChangesAsync(ct);

            return Result.Success();
        }
    }
}
