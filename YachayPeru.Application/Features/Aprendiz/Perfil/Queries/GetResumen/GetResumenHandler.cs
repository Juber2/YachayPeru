using MediatR;
using YachayPeru.Application.Abstractions.Persistence.Aprendiz;
using YachayPeru.Application.Abstractions.Persistence.Auth;
using YachayPeru.Application.Abstractions.Persistence.Learning;
using YachayPeru.Application.Common;

namespace YachayPeru.Application.Features.Aprendiz.Perfil.Queries.GetResumen
{
    public class GetResumenHandler : IRequestHandler<GetResumenQuery, AprendizPerfilResumen>
    {
        private readonly IAprendizProfileRepository profileRepository;
        private readonly IAprendizRegionActivityRepository regionActivityRepository;
        private readonly IAprendizInsigniaEarnedRepository insigniaEarnedRepository;
        private readonly IUserRepository userRepository;
        private readonly ICourseRepository courseRepository;
        private readonly ICourseModuleRepository courseModuleRepository;

        public GetResumenHandler(
            IAprendizProfileRepository _profileRepository,
            IAprendizRegionActivityRepository _regionActivityRepository,
            IAprendizInsigniaEarnedRepository _insigniaEarnedRepository,
            IUserRepository _userRepository,
            ICourseRepository _courseRepository,
            ICourseModuleRepository _courseModuleRepository)
        {
            profileRepository = _profileRepository;
            regionActivityRepository = _regionActivityRepository;
            insigniaEarnedRepository = _insigniaEarnedRepository;
            userRepository = _userRepository;
            courseRepository = _courseRepository;
            courseModuleRepository = _courseModuleRepository;
        }

        public async Task<AprendizPerfilResumen> Handle(GetResumenQuery request, CancellationToken ct)
        {
            var profile = await profileRepository.GetByUserIdAsync(request.UserId, ct);
            var user = await userRepository.GetByIdWithPersonAsync(request.UserId, ct);
            var fullName = user?.Person is null ? string.Empty : $"{user.Person.FirstName} {user.Person.LastName}".Trim();

            var points = profile?.Points ?? 0;
            var level = AprendizLevelCalculator.CalculateLevel(points);

            string? favoriteRegionTitle = null;
            if (profile?.FavoriteRegionId is not null)
            {
                var favoriteRegion = await courseRepository.GetByIdAsync(profile.FavoriteRegionId.Value, ct);
                favoriteRegionTitle = favoriteRegion?.Title;
            }

            var badgeCount = (await insigniaEarnedRepository.GetByUserAsync(request.UserId, ct)).Count;

            UltimaActividad? ultimaActividad = null;
            var lastRegionActivity = await regionActivityRepository.GetByUserIdAsync(request.UserId, ct);
            if (lastRegionActivity is not null)
            {
                var region = await courseRepository.GetByIdAsync(lastRegionActivity.RegionId, ct);
                var module = await courseModuleRepository.GetByIdAsync(lastRegionActivity.ModuleId, ct);
                if (region is not null && module is not null)
                {
                    ultimaActividad = new UltimaActividad
                    {
                        RegionId = region.Id,
                        RegionTitle = region.Title,
                        ModuleId = module.Id,
                        ModuleTitle = module.Title
                    };
                }
            }

            return new AprendizPerfilResumen
            {
                FullName = fullName,
                AvatarUrl = profile?.AvatarUrl,
                Level = level,
                Points = points,
                NextLevelPoints = AprendizLevelCalculator.NextLevelPoints(level),
                BadgeCount = badgeCount,
                UltimaActividad = ultimaActividad,
                IsPremiumUser = profile?.IsPremiumUser ?? false,
                ModulesDone = profile?.ModulesDone ?? 0,
                LearningTimeMinutes = profile?.LearningTimeMinutes ?? 0,
                FavoriteRegionId = profile?.FavoriteRegionId,
                FavoriteRegionTitle = favoriteRegionTitle
            };
        }
    }
}
