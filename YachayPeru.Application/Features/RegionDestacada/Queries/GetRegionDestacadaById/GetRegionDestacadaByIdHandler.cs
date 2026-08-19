using MediatR;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Application.Abstractions.Persistence.Learning;
using YachayPeru.Application.Common.Results;
using static YachayPeru.Application.Common.Results.ResultCodes;

namespace YachayPeru.Application.Features.RegionDestacada.Queries.GetRegionDestacadaById
{
    public class GetRegionDestacadaByIdHandler : IRequestHandler<GetRegionDestacadaByIdQuery, Result<RegionDestacadaDetail>>
    {
        private readonly IRegionDestacadaRepository repository;
        private readonly ICourseRepository courseRepository;

        public GetRegionDestacadaByIdHandler(IRegionDestacadaRepository _repository, ICourseRepository _courseRepository)
        {
            repository = _repository;
            courseRepository = _courseRepository;
        }

        public async Task<Result<RegionDestacadaDetail>> Handle(GetRegionDestacadaByIdQuery request, CancellationToken ct)
        {
            var e = await repository.GetByIdAsync(request.Id, ct);
            if (e is null)
                return Result<RegionDestacadaDetail>.Failure("Región destacada no encontrada.", NotFound);

            var course = await courseRepository.GetByIdAsync(e.CourseId, ct);

            return Result<RegionDestacadaDetail>.Success(new RegionDestacadaDetail
            {
                Id = e.Id,
                RegionId = e.CourseId,
                RegionTitle = course?.Title ?? string.Empty,
                StartDate = e.StartDate,
                EndDate = e.EndDate
            });
        }
    }
}
