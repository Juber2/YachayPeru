using MediatR;
using YachayPeru.Application.Actions.Courses;
using YachayPeru.Application.Actions.Courses.Models;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Retos.Commands.UpsertRetoSettings
{
    public class UpsertRetoSettingsHandler : IRequestHandler<UpsertRetoSettingsCommand, Result<int>>
    {
        private readonly RetoActions retoActions;
        public UpsertRetoSettingsHandler(RetoActions _retoActions) => retoActions = _retoActions;
        public Task<Result<int>> Handle(UpsertRetoSettingsCommand request, CancellationToken ct)
            => retoActions.UpsertRetoSettings(new UpsertRetoSettingsInput
            {
                RetoId = request.RetoId,
                Title = request.Title,
                PassingScore = request.PassingScore,
                TimeLimitMinutes = request.TimeLimitMinutes,
                MaxAttempts = request.MaxAttempts,
                ShuffleQuestionOrder = request.ShuffleQuestionOrder,
                ShuffleOptionOrder = request.ShuffleOptionOrder,
                ShowResultsAtEnd = request.ShowResultsAtEnd
            }, ct);
    }
}
