using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Insignias.Queries.GetInsigniaById
{
    public record GetInsigniaByIdQuery(int Id) : IRequest<Result<InsigniaDetail>>;

    public class InsigniaDetail
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public int? MinPoints { get; set; }
        public List<RegionRequirement> RequiredRegions { get; set; } = [];
        public List<RetoRequirement> RequiredRetos { get; set; } = [];
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

    public class RegionRequirement
    {
        public int RegionId { get; set; }
        public string RegionTitle { get; set; } = string.Empty;
    }

    public class RetoRequirement
    {
        public int RetoId { get; set; }
        public int CourseId { get; set; }
        public string RetoTitle { get; set; } = string.Empty;
        public string RegionTitle { get; set; } = string.Empty;
    }
}
