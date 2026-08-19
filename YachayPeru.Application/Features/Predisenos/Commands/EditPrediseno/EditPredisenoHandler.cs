using MediatR;
using YachayPeru.Application.Abstractions.Persistence;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Application.Common.Results;
using static YachayPeru.Application.Common.Results.ResultCodes;

namespace YachayPeru.Application.Features.Predisenos.Commands.EditPrediseno
{
    public class EditPredisenoHandler : IRequestHandler<EditPredisenoCommand, Result>
    {
        private readonly IPredisenoRepository repository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUser currentUser;

        public EditPredisenoHandler(IPredisenoRepository _repository, IUnitOfWork _unitOfWork, ICurrentUser _currentUser)
        {
            repository = _repository;
            unitOfWork = _unitOfWork;
            currentUser = _currentUser;
        }

        public async Task<Result> Handle(EditPredisenoCommand request, CancellationToken ct)
        {
            var entity = await repository.GetByIdAsync(request.Id, ct);
            if (entity is null)
                return Result.Failure("Prediseño no encontrado.", NotFound);

            entity.Title = request.Title;
            entity.TreeJson = request.TreeJson;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = currentUser.Id;

            repository.Update(entity);
            await unitOfWork.SaveChangesAsync(ct);

            return Result.Success();
        }
    }
}
