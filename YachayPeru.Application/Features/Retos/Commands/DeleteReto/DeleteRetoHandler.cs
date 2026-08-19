using MediatR;
using YachayPeru.Application.Actions.Courses;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Retos.Commands.DeleteReto
{
    public class DeleteRetoHandler : IRequestHandler<DeleteRetoCommand, Result>
    {
        private readonly RetoActions retoActions;
        public DeleteRetoHandler(RetoActions _retoActions) => retoActions = _retoActions;
        public Task<Result> Handle(DeleteRetoCommand request, CancellationToken ct)
            => retoActions.DeleteReto(request.RetoId, ct);
    }
}
