using MediatR;
using YachayPeru.Application.Abstractions.Persistence.Aprendiz;
using YachayPeru.Application.Abstractions.Persistence.Assessment;
using YachayPeru.Application.Abstractions.Persistence.Learning;

namespace YachayPeru.Application.Features.Aprendiz.Regiones.Queries.GetRegiones
{
    public class GetRegionesHandler : IRequestHandler<GetRegionesQuery, IReadOnlyList<AprendizRegionListItem>>
    {
        private readonly ICourseRepository courseRepository;
        private readonly IRetoRepository retoRepository;
        private readonly IRetoVersionRepository versionRepository;
        private readonly IRetoAttemptRepository attemptRepository;

        public GetRegionesHandler(
            ICourseRepository _courseRepository,
            IRetoRepository _retoRepository,
            IRetoVersionRepository _versionRepository,
            IRetoAttemptRepository _attemptRepository)
        {
            courseRepository = _courseRepository;
            retoRepository = _retoRepository;
            versionRepository = _versionRepository;
            attemptRepository = _attemptRepository;
        }

        public async Task<IReadOnlyList<AprendizRegionListItem>> Handle(GetRegionesQuery request, CancellationToken ct)
        {
            var regions = await courseRepository.ListAsync(c => c.IsActive, ct);
            var items = new List<AprendizRegionListItem>();

            foreach (var region in regions)
            {
                var retos = await retoRepository.GetByCourseAsync(region.Id, ct);

                var publishedCount = 0;
                var passedCount = 0;
                foreach (var reto in retos)
                {
                    var published = await versionRepository.GetPublishedByRetoAsync(reto.Id, ct);
                    if (published is null) continue;

                    publishedCount++;
                    if (await attemptRepository.HasPassedAsync(request.UserId, reto.Id, ct))
                        passedCount++;
                }

                var progressPercent = publishedCount == 0 ? 0 : (int)Math.Round(passedCount * 100.0 / publishedCount);

                items.Add(new AprendizRegionListItem
                {
                    Id = region.Id,
                    Title = region.Title,
                    Description = region.Description,
                    CoverImageUrl = region.CoverImageUrl,
                    ProgressPercent = progressPercent,
                    IsCompleted = publishedCount > 0 && passedCount == publishedCount
                });
            }

            return items;
        }
    }
}
