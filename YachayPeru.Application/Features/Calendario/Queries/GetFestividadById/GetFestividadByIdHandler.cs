using MediatR;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Application.Abstractions.Persistence.Learning;
using YachayPeru.Application.Common.Results;
using static YachayPeru.Application.Common.Results.ResultCodes;

namespace YachayPeru.Application.Features.Calendario.Queries.GetFestividadById
{
    public class GetFestividadByIdHandler : IRequestHandler<GetFestividadByIdQuery, Result<FestividadDetail>>
    {
        private readonly IFestividadRepository repository;
        private readonly ICourseRepository courseRepository;

        public GetFestividadByIdHandler(IFestividadRepository _repository, ICourseRepository _courseRepository)
        {
            repository = _repository;
            courseRepository = _courseRepository;
        }

        public async Task<Result<FestividadDetail>> Handle(GetFestividadByIdQuery request, CancellationToken ct)
        {
            var f = await repository.GetByIdAsync(request.Id, ct);
            if (f is null)
                return Result<FestividadDetail>.Failure("Festividad no encontrada.", NotFound);

            var course = await courseRepository.GetByIdAsync(f.CourseId, ct);

            return Result<FestividadDetail>.Success(new FestividadDetail
            {
                Id = f.Id,
                Name = f.Name,
                Description = f.Description,
                RegionId = f.CourseId,
                RegionTitle = course?.Title ?? string.Empty,
                Month = f.Month,
                Day = f.Day,
                IsActive = f.IsActive
            });
        }
    }
}
