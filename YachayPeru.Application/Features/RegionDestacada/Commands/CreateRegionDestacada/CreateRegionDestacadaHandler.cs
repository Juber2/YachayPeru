using MediatR;
using YachayPeru.Application.Abstractions.Persistence;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Application.Common.Results;
using RegionDestacadaEntity = YachayPeru.Domain.Entities.Content.RegionDestacada;

namespace YachayPeru.Application.Features.RegionDestacada.Commands.CreateRegionDestacada
{
    public class CreateRegionDestacadaHandler : IRequestHandler<CreateRegionDestacadaCommand, Result<int>>
    {
        private readonly IRegionDestacadaRepository repository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUser currentUser;

        public CreateRegionDestacadaHandler(IRegionDestacadaRepository _repository, IUnitOfWork _unitOfWork, ICurrentUser _currentUser)
        {
            repository = _repository;
            unitOfWork = _unitOfWork;
            currentUser = _currentUser;
        }

        public async Task<Result<int>> Handle(CreateRegionDestacadaCommand request, CancellationToken ct)
        {
            var entity = new RegionDestacadaEntity
            {
                CourseId = request.RegionId,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = currentUser.Id
            };

            await repository.AddAsync(entity, ct);
            await unitOfWork.SaveChangesAsync(ct);

            return Result<int>.Success(entity.Id);
        }
    }
}
