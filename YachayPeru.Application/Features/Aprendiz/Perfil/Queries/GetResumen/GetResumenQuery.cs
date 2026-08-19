using MediatR;

namespace YachayPeru.Application.Features.Aprendiz.Perfil.Queries.GetResumen
{
    public record GetResumenQuery(int UserId) : IRequest<AprendizPerfilResumen>;

    public class AprendizPerfilResumen
    {
        public string FullName { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; }
        public int Level { get; set; }
        public int Points { get; set; }
        public int NextLevelPoints { get; set; }
        public int BadgeCount { get; set; }
        public UltimaActividad? UltimaActividad { get; set; }
        public bool IsPremiumUser { get; set; }
        public int ModulesDone { get; set; }
        public int LearningTimeMinutes { get; set; }
        public int? FavoriteRegionId { get; set; }
        public string? FavoriteRegionTitle { get; set; }
    }

    public class UltimaActividad
    {
        public int RegionId { get; set; }
        public string RegionTitle { get; set; } = string.Empty;
        public int ModuleId { get; set; }
        public string ModuleTitle { get; set; } = string.Empty;
    }
}
