using MediatR;
using YachayPeru.Application.Actions.Courses;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Retos.Commands.DiscardRetoDraft
{
    public class DiscardRetoDraftHandler : IRequestHandler<DiscardRetoDraftCommand, Result>
    {
        private readonly RetoActions retoActions;
        public DiscardRetoDraftHandler(RetoActions _retoActions) => retoActions = _retoActions;
        public Task<Result> Handle(DiscardRetoDraftCommand request, CancellationToken ct)
            => retoActions.DiscardRetoDraft(request.RetoId, ct);
    }
}
