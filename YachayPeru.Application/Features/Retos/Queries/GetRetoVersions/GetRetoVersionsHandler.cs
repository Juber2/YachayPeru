using MediatR;
using YachayPeru.Application.Actions.Courses;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Retos.Queries.GetRetoVersions
{
    public class GetRetoVersionsHandler : IRequestHandler<GetRetoVersionsQuery, IReadOnlyList<RetoVersionSummary>>
    {
        private readonly RetoActions retoActions;
        public GetRetoVersionsHandler(RetoActions _retoActions) => retoActions = _retoActions;
        public Task<IReadOnlyList<RetoVersionSummary>> Handle(GetRetoVersionsQuery request, CancellationToken ct)
            => retoActions.GetRetoVersionHistory(request.RetoId, ct);
    }

    public class GetRetoVersionDetailHandler : IRequestHandler<GetRetoVersionDetailQuery, Result<RetoVersionDetail>>
    {
        private readonly RetoActions retoActions;
        public GetRetoVersionDetailHandler(RetoActions _retoActions) => retoActions = _retoActions;
        public Task<Result<RetoVersionDetail>> Handle(GetRetoVersionDetailQuery request, CancellationToken ct)
            => retoActions.GetRetoVersionDetail(request.VersionId, request.RetoId, ct);
    }
}
