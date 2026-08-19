using YachayPeru.Domain.Base;

namespace YachayPeru.Domain.Entities.Content
{
    public class Insignia : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; } = true;
        public int? MinPoints { get; set; }
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
