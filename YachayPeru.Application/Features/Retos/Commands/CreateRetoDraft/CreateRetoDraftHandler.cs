using MediatR;
using YachayPeru.Application.Actions.Courses;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Retos.Commands.CreateRetoDraft
{
    public class CreateRetoDraftHandler : IRequestHandler<CreateRetoDraftCommand, Result<int>>
    {
        private readonly RetoActions retoActions;
        public CreateRetoDraftHandler(RetoActions _retoActions) => retoActions = _retoActions;
        public Task<Result<int>> Handle(CreateRetoDraftCommand request, CancellationToken ct)
            => retoActions.CreateRetoDraft(request.RetoId, ct);
    }
}
