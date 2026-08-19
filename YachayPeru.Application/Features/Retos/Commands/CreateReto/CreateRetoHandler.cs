using MediatR;
using YachayPeru.Application.Actions.Courses;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Retos.Commands.CreateReto
{
    public class CreateRetoHandler : IRequestHandler<CreateRetoCommand, Result<int>>
    {
        private readonly RetoActions retoActions;
        public CreateRetoHandler(RetoActions _retoActions) => retoActions = _retoActions;
        public Task<Result<int>> Handle(CreateRetoCommand request, CancellationToken ct)
            => retoActions.CreateReto(request.CourseId, ct);
    }
}
