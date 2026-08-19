namespace YachayPeru.API.Contracts.Aprendiz.Perfil.Response
{
    public record UltimaActividadResponse
    {
        public int RegionId { get; init; }
        public string RegionTitle { get; init; } = string.Empty;
        public int ModuleId { get; init; }
        public string ModuleTitle { get; init; } = string.Empty;
    }

    public record AprendizPerfilResumenResponse
    {
        public string FullName { get; init; } = string.Empty;
        public string? AvatarUrl { get; init; }
        public int Level { get; init; }
        public int Points { get; init; }
        public int NextLevelPoints { get; init; }
        public int BadgeCount { get; init; }
        public UltimaActividadResponse? UltimaActividad { get; init; }
        public bool IsPremiumUser { get; init; }
        public int ModulesDone { get; init; }
        public int LearningTimeMinutes { get; init; }
        public int? FavoriteRegionId { get; init; }
        public string? FavoriteRegionTitle { get; init; }
    }

    public record AprendizActividadItemResponse
    {
        public string Text { get; init; } = string.Empty;
        public string When { get; init; } = string.Empty;
    }
}
