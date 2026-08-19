using MediatR;
using YachayPeru.Application.Abstractions.Persistence;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Application.Common.Results;
using static YachayPeru.Application.Common.Results.ResultCodes;

namespace YachayPeru.Application.Features.Biblioteca.Commands.DeleteMediaItem
{
    public class DeleteMediaItemHandler : IRequestHandler<DeleteMediaItemCommand, Result>
    {
        private readonly IMediaItemRepository repository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUser currentUser;

        public DeleteMediaItemHandler(IMediaItemRepository _repository, IUnitOfWork _unitOfWork, ICurrentUser _currentUser)
        {
            repository = _repository;
            unitOfWork = _unitOfWork;
            currentUser = _currentUser;
        }

        public async Task<Result> Handle(DeleteMediaItemCommand request, CancellationToken ct)
        {
            var item = await repository.GetByIdAsync(request.Id, ct);
            if (item is null)
                return Result.Failure("Recurso no encontrado.", NotFound);

            item.Deleted = true;
            item.UpdatedAt = DateTime.UtcNow;
            item.UpdatedBy = currentUser.Id;

            repository.Update(item);
            await unitOfWork.SaveChangesAsync(ct);

            return Result.Success();
        }
    }
}
