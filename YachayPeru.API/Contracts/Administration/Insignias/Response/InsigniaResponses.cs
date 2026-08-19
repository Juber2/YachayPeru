namespace YachayPeru.API.Contracts.Administration.Insignias.Response
{
    public record InsigniaListItemResponse
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string? ImageUrl { get; init; }
        public bool IsActive { get; init; }
        public int? MinPoints { get; init; }
        public int RequiredRegionCount { get; init; }
        public int RequiredRetoCount { get; init; }
        public int? MinRetosCompleted { get; init; }
        public int? MinPerfectRetos { get; init; }
        public bool RequireAllQuestionTypes { get; init; }
        public int? MinLevel { get; init; }
        public bool RequirePremium { get; init; }
        public int? MinLoginStreakDays { get; init; }
        public int? MinAnswerStreak { get; init; }
        public int? MinRegionsExplored { get; init; }
        public string? RequiredZoneCode { get; init; }
        public int? MinZoneRegionsExplored { get; init; }
        public DateTime CreatedAt { get; init; }
    }

    public record RegionRequirementResponse
    {
        public int RegionId { get; init; }
        public string RegionTitle { get; init; } = string.Empty;
    }

    public record RetoRequirementResponse
    {
        public int RetoId { get; init; }
        public int CourseId { get; init; }
        public string RetoTitle { get; init; } = string.Empty;
        public string RegionTitle { get; init; } = string.Empty;
    }

    public record InsigniaDetailResponse
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string? ImageUrl { get; init; }
        public bool IsActive { get; init; }
        public int? MinPoints { get; init; }
        public List<RegionRequirementResponse> RequiredRegions { get; init; } = [];
        public List<RetoRequirementResponse> RequiredRetos { get; init; } = [];
        public int? MinRetosCompleted { get; init; }
        public int? MinPerfectRetos { get; init; }
        public bool RequireAllQuestionTypes { get; init; }
        public int? MinLevel { get; init; }
        public bool RequirePremium { get; init; }
        public int? MinLoginStreakDays { get; init; }
        public int? MinAnswerStreak { get; init; }
        public int? MinRegionsExplored { get; init; }
        public string? RequiredZoneCode { get; init; }
        public int? MinZoneRegionsExplored { get; init; }
    }
}
