using MediatR;
using YachayPeru.Application.Actions.Courses;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Retos.Commands.PublishReto
{
    public class PublishRetoHandler : IRequestHandler<PublishRetoCommand, Result<int>>
    {
        private readonly RetoActions retoActions;
        public PublishRetoHandler(RetoActions _retoActions) => retoActions = _retoActions;
        public Task<Result<int>> Handle(PublishRetoCommand request, CancellationToken ct)
            => retoActions.PublishReto(request.RetoId, ct);
    }
}
