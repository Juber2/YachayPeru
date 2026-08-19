using MediatR;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Application.Abstractions.Persistence.Learning;

namespace YachayPeru.Application.Features.Aprendiz.RegionDestacada.Queries.GetRegionDestacadaActual
{
    public class GetRegionDestacadaActualHandler : IRequestHandler<GetRegionDestacadaActualQuery, AprendizRegionDestacadaItem?>
    {
        private readonly IRegionDestacadaRepository repository;
        private readonly ICourseRepository courseRepository;

        public GetRegionDestacadaActualHandler(IRegionDestacadaRepository _repository, ICourseRepository _courseRepository)
        {
            repository = _repository;
            courseRepository = _courseRepository;
        }

        public async Task<AprendizRegionDestacadaItem?> Handle(GetRegionDestacadaActualQuery request, CancellationToken ct)
        {
            var now = DateTime.UtcNow;
            var candidates = await repository.ListAsync(x => x.StartDate <= now && x.EndDate >= now, ct);

            var current = candidates.OrderByDescending(x => x.StartDate).FirstOrDefault();
            if (current is null) return null;

            var region = await courseRepository.GetByIdAsync(current.CourseId, ct);
            if (region is null) return null;

            return new AprendizRegionDestacadaItem
            {
                RegionId = region.Id,
                RegionTitle = region.Title,
                RegionDescription = region.Description,
                CoverImageUrl = region.CoverImageUrl,
                StartDate = current.StartDate,
                EndDate = current.EndDate
            };
        }
    }
}
