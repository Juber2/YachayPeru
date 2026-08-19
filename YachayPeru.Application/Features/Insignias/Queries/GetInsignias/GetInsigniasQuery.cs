using MediatR;

namespace YachayPeru.Application.Features.Insignias.Queries.GetInsignias
{
    public record GetInsigniasQuery : IRequest<IReadOnlyList<InsigniaListItem>>;

    public class InsigniaListItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public int? MinPoints { get; set; }
        public int RequiredRegionCount { get; set; }
        public int RequiredRetoCount { get; set; }
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
        public DateTime CreatedAt { get; set; }
    }
}
