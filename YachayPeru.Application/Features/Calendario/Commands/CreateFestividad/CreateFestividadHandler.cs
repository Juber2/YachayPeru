using MediatR;
using YachayPeru.Application.Abstractions.Persistence;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Application.Common.Results;
using YachayPeru.Domain.Entities.Content;

namespace YachayPeru.Application.Features.Calendario.Commands.CreateFestividad
{
    public class CreateFestividadHandler : IRequestHandler<CreateFestividadCommand, Result<int>>
    {
        private readonly IFestividadRepository repository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUser currentUser;

        public CreateFestividadHandler(IFestividadRepository _repository, IUnitOfWork _unitOfWork, ICurrentUser _currentUser)
        {
            repository = _repository;
            unitOfWork = _unitOfWork;
            currentUser = _currentUser;
        }

        public async Task<Result<int>> Handle(CreateFestividadCommand request, CancellationToken ct)
        {
            var festividad = new Festividad
            {
                Name = request.Name,
                Description = request.Description,
                CourseId = request.RegionId,
                Month = request.Month,
                Day = request.Day,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = currentUser.Id
            };

            await repository.AddAsync(festividad, ct);
            await unitOfWork.SaveChangesAsync(ct);

            return Result<int>.Success(festividad.Id);
        }
    }
}
