using MediatR;
using YachayPeru.Application.Abstractions.Persistence;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Application.Common.Results;
using static YachayPeru.Application.Common.Results.ResultCodes;

namespace YachayPeru.Application.Features.Calendario.Commands.DeleteFestividad
{
    public class DeleteFestividadHandler : IRequestHandler<DeleteFestividadCommand, Result>
    {
        private readonly IFestividadRepository repository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUser currentUser;

        public DeleteFestividadHandler(IFestividadRepository _repository, IUnitOfWork _unitOfWork, ICurrentUser _currentUser)
        {
            repository = _repository;
            unitOfWork = _unitOfWork;
            currentUser = _currentUser;
        }

        public async Task<Result> Handle(DeleteFestividadCommand request, CancellationToken ct)
        {
            var festividad = await repository.GetByIdAsync(request.Id, ct);
            if (festividad is null)
                return Result.Failure("Festividad no encontrada.", NotFound);

            festividad.Deleted = true;
            festividad.UpdatedAt = DateTime.UtcNow;
            festividad.UpdatedBy = currentUser.Id;

            repository.Update(festividad);
            await unitOfWork.SaveChangesAsync(ct);

            return Result.Success();
        }
    }
}
