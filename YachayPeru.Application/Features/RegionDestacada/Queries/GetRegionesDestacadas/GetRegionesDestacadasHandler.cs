using MediatR;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Application.Abstractions.Persistence.Learning;

namespace YachayPeru.Application.Features.RegionDestacada.Queries.GetRegionesDestacadas
{
    public class GetRegionesDestacadasHandler : IRequestHandler<GetRegionesDestacadasQuery, IReadOnlyList<RegionDestacadaListItem>>
    {
        private readonly IRegionDestacadaRepository repository;
        private readonly ICourseRepository courseRepository;

        public GetRegionesDestacadasHandler(IRegionDestacadaRepository _repository, ICourseRepository _courseRepository)
        {
            repository = _repository;
            courseRepository = _courseRepository;
        }

        public async Task<IReadOnlyList<RegionDestacadaListItem>> Handle(GetRegionesDestacadasQuery request, CancellationToken ct)
        {
            var entities = await repository.ListAsync(ct);
            var items = new List<RegionDestacadaListItem>();

            foreach (var e in entities)
            {
                var course = await courseRepository.GetByIdAsync(e.CourseId, ct);
                items.Add(new RegionDestacadaListItem
                {
                    Id = e.Id,
                    RegionId = e.CourseId,
                    RegionTitle = course?.Title ?? string.Empty,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate
                });
            }

            return items;
        }
    }
}
