using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Insignias.Commands.EditInsignia
{
    public sealed record EditInsigniaCommand : IRequest<Result>
    {
        public int Id { get; init; }
        public string Name { get; init; } = default!;
        public string Description { get; init; } = default!;
        public bool IsActive { get; init; }
        public int? MinPoints { get; init; }
        public List<int> RequiredRegionIds { get; init; } = [];
        public List<int> RequiredRetoIds { get; init; } = [];
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
