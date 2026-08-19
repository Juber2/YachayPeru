using MediatR;
using YachayPeru.Application.Actions.Courses;

namespace YachayPeru.Application.Features.Retos.Queries.GetRetosLookup
{
    public class GetRetosLookupHandler : IRequestHandler<GetRetosLookupQuery, IReadOnlyList<RetoLookupItem>>
    {
        private readonly RetoActions retoActions;
        public GetRetosLookupHandler(RetoActions _retoActions) => retoActions = _retoActions;
        public Task<IReadOnlyList<RetoLookupItem>> Handle(GetRetosLookupQuery request, CancellationToken ct)
            => retoActions.GetRetosLookup(ct);
    }
}
