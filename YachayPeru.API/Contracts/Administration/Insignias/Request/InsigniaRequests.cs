namespace YachayPeru.API.Contracts.Administration.Insignias.Request
{
    public class UpsertInsigniaRequest
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public bool IsActive { get; set; } = true;
        public int? MinPoints { get; set; }
        public List<int> RequiredRegionIds { get; set; } = [];
        public List<int> RequiredRetoIds { get; set; } = [];
        public int? MinRetosCompleted { get; set; }
        public int? MinPerfectRetos { get; set; }
        public bool RequireAllQuestionTypes { get; set; }
        public int? MinLevel { get; set; }
        public bool RequirePremium { get; set; }
        public int? MinLoginStreakDays { get; set; }
        public int? MinAnswerStreak { get; set; }
        public int? MinRegionsExplored { get; set; }
        public string? RequiredZoneCode { get; set; }
        public int? MinZoneRegionsExplored { get; set; }
    }
}
