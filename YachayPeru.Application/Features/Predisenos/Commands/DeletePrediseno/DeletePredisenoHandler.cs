using MediatR;
using YachayPeru.Application.Abstractions.Persistence;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Application.Common.Results;
using static YachayPeru.Application.Common.Results.ResultCodes;

namespace YachayPeru.Application.Features.Predisenos.Commands.DeletePrediseno
{
    public class DeletePredisenoHandler : IRequestHandler<DeletePredisenoCommand, Result>
    {
        private readonly IPredisenoRepository repository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUser currentUser;

        public DeletePredisenoHandler(IPredisenoRepository _repository, IUnitOfWork _unitOfWork, ICurrentUser _currentUser)
        {
            repository = _repository;
            unitOfWork = _unitOfWork;
            currentUser = _currentUser;
        }

        public async Task<Result> Handle(DeletePredisenoCommand request, CancellationToken ct)
        {
            var entity = await repository.GetByIdAsync(request.Id, ct);
            if (entity is null)
                return Result.Failure("Prediseño no encontrado.", NotFound);

            entity.Deleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = currentUser.Id;

            repository.Update(entity);
            await unitOfWork.SaveChangesAsync(ct);

            return Result.Success();
        }
    }
}
