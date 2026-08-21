using MediatR;
using YachayPeru.Application.Abstractions.Persistence.Aprendiz;
using YachayPeru.Application.Abstractions.Persistence.Assessment;
using YachayPeru.Application.Abstractions.Persistence.Learning;
using YachayPeru.Domain.Constants;

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
            var regionIds = regions.Select(r => r.Id).ToList();

            // 4 consultas en total (sin importar cuántas regiones/retos haya) en vez de las
            // ~2 consultas por reto que hacía antes este método — esa era la causa real de la
            // demora al abrir "Explorar" en el servidor.
            var allRetos = await retoRepository.ListAsync(r => regionIds.Contains(r.CourseId), ct);
            var retoIds = allRetos.Select(r => r.Id).ToList();

            var publishedVersions = await versionRepository.ListAsync(
                v => retoIds.Contains(v.RetoId) && v.StatusCode == AppConstants.RetoVersionStatus.Published, ct);
            var publishedRetoIds = publishedVersions.Select(v => v.RetoId).ToHashSet();

            var passedRetoIds = (await attemptRepository.GetPassedRetoIdsByUserAsync(request.UserId, ct)).ToHashSet();

            var retosByRegion = allRetos.GroupBy(r => r.CourseId).ToDictionary(g => g.Key, g => g.ToList());

            var items = new List<AprendizRegionListItem>();
            foreach (var region in regions)
            {
                var retos = retosByRegion.TryGetValue(region.Id, out var list) ? list : [];
                var publishedCount = retos.Count(r => publishedRetoIds.Contains(r.Id));
                var passedCount = retos.Count(r => publishedRetoIds.Contains(r.Id) && passedRetoIds.Contains(r.Id));

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
