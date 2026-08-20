using YachayPeru.Application.Abstractions.Persistence;
using YachayPeru.Application.Abstractions.Persistence.Aprendiz;
using YachayPeru.Application.Abstractions.Persistence.Assessment;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Application.Abstractions.Persistence.Learning;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Domain.Constants;
using YachayPeru.Domain.Entities.Aprendiz;

namespace YachayPeru.Application.Actions.Aprendiz
{
    public class InsigniaEvaluator : IInsigniaEvaluator
    {
        private readonly IInsigniaRepository insigniaRepository;
        private readonly IAprendizInsigniaEarnedRepository earnedRepository;
        private readonly IAprendizProfileRepository profileRepository;
        private readonly IAprendizActivityLogRepository activityRepository;
        private readonly IRetoRepository retoRepository;
        private readonly IRetoVersionRepository versionRepository;
        private readonly IRetoAttemptRepository attemptRepository;
        private readonly IRetoVersionQuestionRepository questionRepository;
        private readonly IAprendizRegionExploredRepository regionExploredRepository;
        private readonly ICourseRepository courseRepository;
        private readonly IUnitOfWork unitOfWork;

        public InsigniaEvaluator(
            IInsigniaRepository _insigniaRepository,
            IAprendizInsigniaEarnedRepository _earnedRepository,
            IAprendizProfileRepository _profileRepository,
            IAprendizActivityLogRepository _activityRepository,
            IRetoRepository _retoRepository,
            IRetoVersionRepository _versionRepository,
            IRetoAttemptRepository _attemptRepository,
            IRetoVersionQuestionRepository _questionRepository,
            IAprendizRegionExploredRepository _regionExploredRepository,
            ICourseRepository _courseRepository,
            IUnitOfWork _unitOfWork)
        {
            insigniaRepository = _insigniaRepository;
            earnedRepository = _earnedRepository;
            profileRepository = _profileRepository;
            activityRepository = _activityRepository;
            retoRepository = _retoRepository;
            versionRepository = _versionRepository;
            attemptRepository = _attemptRepository;
            questionRepository = _questionRepository;
            regionExploredRepository = _regionExploredRepository;
            courseRepository = _courseRepository;
            unitOfWork = _unitOfWork;
        }

        public async Task EvaluateAsync(int userId, CancellationToken ct = default)
        {
            var profile = await profileRepository.GetByUserIdAsync(userId, ct);
            var points = profile?.Points ?? 0;

            var insignias = await insigniaRepository.ListAsync(x => x.IsActive, ct);
            var passedRetoIds = (await attemptRepository.GetPassedRetoIdsByUserAsync(userId, ct)).ToHashSet();
            var regionCompletionCache = new Dictionary<int, bool>();

            // Datos que solo se calculan si alguna insignia activa los necesita (evita trabajo de más).
            int? perfectRetoCount = null;
            IReadOnlyList<string>? passedQuestionTypeCodes = null;
            IReadOnlyList<int>? exploredRegionIds = null;
            Dictionary<int, string?>? exploredRegionZones = null;

            var changed = false;

            foreach (var insignia in insignias)
            {
                if (await earnedRepository.HasEarnedAsync(userId, insignia.Id, ct))
                    continue;

                if (insignia.MinPoints is not null && points < insignia.MinPoints)
                    continue;

                var requiredRegionIds = await insigniaRepository.GetRequiredRegionIdsAsync(insignia.Id, ct);
                var regionsOk = true;
                foreach (var regionId in requiredRegionIds)
                {
                    if (!regionCompletionCache.TryGetValue(regionId, out var completed))
                    {
                        completed = await IsRegionCompletedAsync(regionId, passedRetoIds, ct);
                        regionCompletionCache[regionId] = completed;
                    }

                    if (!completed) { regionsOk = false; break; }
                }
                if (!regionsOk) continue;

                var requiredRetoIds = await insigniaRepository.GetRequiredRetoIdsAsync(insignia.Id, ct);
                if (requiredRetoIds.Any(retoId => !passedRetoIds.Contains(retoId)))
                    continue;

                if (insignia.MinRetosCompleted is not null && passedRetoIds.Count < insignia.MinRetosCompleted)
                    continue;

                if (insignia.MinPerfectRetos is not null)
                {
                    perfectRetoCount ??= await attemptRepository.CountDistinctPerfectRetosByUserAsync(userId, ct);
                    if (perfectRetoCount < insignia.MinPerfectRetos) continue;
                }

                if (insignia.RequireAllQuestionTypes)
                {
                    passedQuestionTypeCodes ??= await GetPassedQuestionTypeCodesAsync(passedRetoIds, ct);
                    var allTypes = new[]
                    {
                        AppConstants.QuestionTypeCode.SingleChoice,
                        AppConstants.QuestionTypeCode.MultipleChoice,
                        AppConstants.QuestionTypeCode.TrueFalse,
                        AppConstants.QuestionTypeCode.FillBlank
                    };
                    if (allTypes.Any(t => !passedQuestionTypeCodes.Contains(t))) continue;
                }

                if (insignia.MinLevel is not null && (profile?.Level ?? 1) < insignia.MinLevel)
                    continue;

                if (insignia.RequirePremium && !(profile?.IsPremiumUser ?? false))
                    continue;

                if (insignia.MinLoginStreakDays is not null && (profile?.BestLoginStreakDays ?? 0) < insignia.MinLoginStreakDays)
                    continue;

                if (insignia.MinAnswerStreak is not null && (profile?.BestAnswerStreak ?? 0) < insignia.MinAnswerStreak)
                    continue;

                if (insignia.MinRegionsExplored is not null)
                {
                    exploredRegionIds ??= await regionExploredRepository.GetExploredRegionIdsAsync(userId, ct);
                    if (exploredRegionIds.Count < insignia.MinRegionsExplored) continue;
                }

                if (insignia.RequiredZoneCode is not null && insignia.MinZoneRegionsExplored is not null)
                {
                    exploredRegionIds ??= await regionExploredRepository.GetExploredRegionIdsAsync(userId, ct);
                    exploredRegionZones ??= await GetZonesByRegionIdsAsync(exploredRegionIds, ct);

                    var zoneCount = exploredRegionIds.Count(regionId =>
                        exploredRegionZones.TryGetValue(regionId, out var zone) && zone == insignia.RequiredZoneCode);
                    if (zoneCount < insignia.MinZoneRegionsExplored) continue;
                }

                await earnedRepository.AddAsync(new AprendizInsigniaEarned
                {
                    UserId = userId,
                    InsigniaId = insignia.Id,
                    EarnedAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = userId
                }, ct);

                await activityRepository.AddAsync(new Domain.Entities.Aprendiz.AprendizActivityLog
                {
                    UserId = userId,
                    Text = $"Ganaste la insignia {insignia.Name}",
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = userId
                }, ct);

                changed = true;
            }

            if (changed)
                await unitOfWork.SaveChangesAsync(ct);
        }

        private async Task<bool> IsRegionCompletedAsync(int regionId, HashSet<int> passedRetoIds, CancellationToken ct)
        {
            var retos = await retoRepository.GetByCourseAsync(regionId, ct);
            var publishedCount = 0;

            foreach (var reto in retos)
            {
                var published = await versionRepository.GetPublishedByRetoAsync(reto.Id, ct);
                if (published is null) continue;

                publishedCount++;
                if (!passedRetoIds.Contains(reto.Id))
                    return false;
            }

            return publishedCount > 0;
        }

        private async Task<IReadOnlyList<string>> GetPassedQuestionTypeCodesAsync(HashSet<int> passedRetoIds, CancellationToken ct)
        {
            var publishedVersions = await versionRepository.ListAsync(
                v => passedRetoIds.Contains(v.RetoId) && v.StatusCode == AppConstants.RetoVersionStatus.Published, ct);

            var versionIds = publishedVersions
                .GroupBy(v => v.RetoId)
                .Select(g => g.OrderByDescending(v => v.VersionNumber).First().Id)
                .ToList();

            if (versionIds.Count == 0) return [];
            return await questionRepository.GetDistinctQuestionTypeCodesByRetoVersionIdsAsync(versionIds, ct);
        }

        private async Task<Dictionary<int, string?>> GetZonesByRegionIdsAsync(IReadOnlyList<int> regionIds, CancellationToken ct)
        {
            var courses = await courseRepository.ListAsync(c => regionIds.Contains(c.Id), ct);
            return courses.ToDictionary(c => c.Id, c => c.ZoneCode);
        }
    }
}
