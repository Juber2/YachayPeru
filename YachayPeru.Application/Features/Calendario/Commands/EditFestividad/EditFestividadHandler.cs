using MediatR;
using YachayPeru.Application.Abstractions.Persistence;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Application.Common.Results;
using static YachayPeru.Application.Common.Results.ResultCodes;

namespace YachayPeru.Application.Features.Calendario.Commands.EditFestividad
{
    public class EditFestividadHandler : IRequestHandler<EditFestividadCommand, Result>
    {
        private readonly IFestividadRepository repository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUser currentUser;

        public EditFestividadHandler(IFestividadRepository _repository, IUnitOfWork _unitOfWork, ICurrentUser _currentUser)
        {
            repository = _repository;
            unitOfWork = _unitOfWork;
            currentUser = _currentUser;
        }

        public async Task<Result> Handle(EditFestividadCommand request, CancellationToken ct)
        {
            var festividad = await repository.GetByIdAsync(request.Id, ct);
            if (festividad is null)
                return Result.Failure("Festividad no encontrada.", NotFound);

            festividad.Name = request.Name;
            festividad.Description = request.Description;
            festividad.CourseId = request.RegionId;
            festividad.Month = request.Month;
            festividad.Day = request.Day;
            festividad.IsActive = request.IsActive;
            festividad.UpdatedAt = DateTime.UtcNow;
            festividad.UpdatedBy = currentUser.Id;

            repository.Update(festividad);
            await unitOfWork.SaveChangesAsync(ct);

            return Result.Success();
        }
    }
}
