using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Retos.Commands.UpsertRetoSettings
{
    public sealed record UpsertRetoSettingsCommand : IRequest<Result<int>>
    {
        public int RetoId { get; init; }
        public string Title { get; init; } = default!;
        public decimal PassingScore { get; init; }
        public int? TimeLimitMinutes { get; init; }
        public int MaxAttempts { get; init; } = 3;
        public bool ShuffleQuestionOrder { get; init; }
        public bool ShuffleOptionOrder { get; init; }
        public bool ShowResultsAtEnd { get; init; } = true;
    }
}
