using MediatR;
using YachayPeru.Application.Abstractions.Persistence;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Application.Common.Results;
using static YachayPeru.Application.Common.Results.ResultCodes;

namespace YachayPeru.Application.Features.RegionDestacada.Commands.EditRegionDestacada
{
    public class EditRegionDestacadaHandler : IRequestHandler<EditRegionDestacadaCommand, Result>
    {
        private readonly IRegionDestacadaRepository repository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUser currentUser;

        public EditRegionDestacadaHandler(IRegionDestacadaRepository _repository, IUnitOfWork _unitOfWork, ICurrentUser _currentUser)
        {
            repository = _repository;
            unitOfWork = _unitOfWork;
            currentUser = _currentUser;
        }

        public async Task<Result> Handle(EditRegionDestacadaCommand request, CancellationToken ct)
        {
            var entity = await repository.GetByIdAsync(request.Id, ct);
            if (entity is null)
                return Result.Failure("Región destacada no encontrada.", NotFound);

            entity.CourseId = request.RegionId;
            entity.StartDate = request.StartDate;
            entity.EndDate = request.EndDate;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = currentUser.Id;

            repository.Update(entity);
            await unitOfWork.SaveChangesAsync(ct);

            return Result.Success();
        }
    }
}
