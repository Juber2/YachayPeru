using MediatR;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Application.Abstractions.Persistence.Learning;

namespace YachayPeru.Application.Features.Calendario.Queries.GetFestividades
{
    public class GetFestividadesHandler : IRequestHandler<GetFestividadesQuery, IReadOnlyList<FestividadListItem>>
    {
        private readonly IFestividadRepository repository;
        private readonly ICourseRepository courseRepository;

        public GetFestividadesHandler(IFestividadRepository _repository, ICourseRepository _courseRepository)
        {
            repository = _repository;
            courseRepository = _courseRepository;
        }

        public async Task<IReadOnlyList<FestividadListItem>> Handle(GetFestividadesQuery request, CancellationToken ct)
        {
            var festividades = await repository.ListAsync(ct);
            var items = new List<FestividadListItem>();

            foreach (var f in festividades)
            {
                var course = await courseRepository.GetByIdAsync(f.CourseId, ct);
                items.Add(new FestividadListItem
                {
                    Id = f.Id,
                    Name = f.Name,
                    RegionId = f.CourseId,
                    RegionTitle = course?.Title ?? string.Empty,
                    Month = f.Month,
                    Day = f.Day,
                    IsActive = f.IsActive,
                    CreatedAt = f.CreatedAt
                });
            }

            return items;
        }
    }
}
