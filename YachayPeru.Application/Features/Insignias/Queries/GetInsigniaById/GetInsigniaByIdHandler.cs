using MediatR;
using YachayPeru.Application.Abstractions.Persistence.Assessment;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Application.Abstractions.Persistence.Learning;
using YachayPeru.Application.Common.Results;
using static YachayPeru.Application.Common.Results.ResultCodes;

namespace YachayPeru.Application.Features.Insignias.Queries.GetInsigniaById
{
    public class GetInsigniaByIdHandler : IRequestHandler<GetInsigniaByIdQuery, Result<InsigniaDetail>>
    {
        private readonly IInsigniaRepository repository;
        private readonly ICourseRepository courseRepository;
        private readonly IRetoRepository retoRepository;
        private readonly IRetoVersionRepository versionRepository;

        public GetInsigniaByIdHandler(
            IInsigniaRepository _repository,
            ICourseRepository _courseRepository,
            IRetoRepository _retoRepository,
            IRetoVersionRepository _versionRepository)
        {
            repository = _repository;
            courseRepository = _courseRepository;
            retoRepository = _retoRepository;
            versionRepository = _versionRepository;
        }

        public async Task<Result<InsigniaDetail>> Handle(GetInsigniaByIdQuery request, CancellationToken ct)
        {
            var insignia = await repository.GetByIdAsync(request.Id, ct);
            if (insignia is null)
                return Result<InsigniaDetail>.Failure("Insignia no encontrada.", NotFound);

            var regionIds = await repository.GetRequiredRegionIdsAsync(insignia.Id, ct);
            var requiredRegions = new List<RegionRequirement>();
            foreach (var regionId in regionIds)
            {
                var course = await courseRepository.GetByIdAsync(regionId, ct);
                requiredRegions.Add(new RegionRequirement
                {
                    RegionId = regionId,
                    RegionTitle = course?.Title ?? string.Empty
                });
            }

            var retoIds = await repository.GetRequiredRetoIdsAsync(insignia.Id, ct);
            var requiredRetos = new List<RetoRequirement>();
            foreach (var retoId in retoIds)
            {
                var reto = await retoRepository.GetByIdAsync(retoId, ct);
                if (reto is null) continue;

                var version = await versionRepository.GetDraftByRetoAsync(retoId, ct)
                    ?? await versionRepository.GetPublishedByRetoAsync(retoId, ct);
                var course = await courseRepository.GetByIdAsync(reto.CourseId, ct);

                requiredRetos.Add(new RetoRequirement
                {
                    RetoId = retoId,
                    CourseId = reto.CourseId,
                    RetoTitle = version?.Title ?? string.Empty,
                    RegionTitle = course?.Title ?? string.Empty
                });
            }

            return Result<InsigniaDetail>.Success(new InsigniaDetail
            {
                Id = insignia.Id,
                Name = insignia.Name,
                Description = insignia.Description,
                ImageUrl = insignia.ImageUrl,
                IsActive = insignia.IsActive,
                MinPoints = insignia.MinPoints,
                RequiredRegions = requiredRegions,
                RequiredRetos = requiredRetos,
                MinRetosCompleted = insignia.MinRetosCompleted,
                MinPerfectRetos = insignia.MinPerfectRetos,
                RequireAllQuestionTypes = insignia.RequireAllQuestionTypes,
                MinLevel = insignia.MinLevel,
                RequirePremium = insignia.RequirePremium,
                MinLoginStreakDays = insignia.MinLoginStreakDays,
                MinAnswerStreak = insignia.MinAnswerStreak,
                MinRegionsExplored = insignia.MinRegionsExplored,
                RequiredZoneCode = insignia.RequiredZoneCode,
                MinZoneRegionsExplored = insignia.MinZoneRegionsExplored
            });
        }
    }
}
