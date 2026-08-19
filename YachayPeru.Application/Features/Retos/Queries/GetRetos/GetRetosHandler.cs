using MediatR;
using YachayPeru.Application.Actions.Courses;

namespace YachayPeru.Application.Features.Retos.Queries.GetRetos
{
    public class GetRetosHandler : IRequestHandler<GetRetosQuery, IReadOnlyList<RetoListItem>>
    {
        private readonly RetoActions retoActions;
        public GetRetosHandler(RetoActions _retoActions) => retoActions = _retoActions;
        public Task<IReadOnlyList<RetoListItem>> Handle(GetRetosQuery request, CancellationToken ct)
            => retoActions.GetRetos(request.CourseId, ct);
    }
}
