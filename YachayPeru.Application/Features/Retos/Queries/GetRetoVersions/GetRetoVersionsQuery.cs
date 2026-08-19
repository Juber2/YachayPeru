using MediatR;
using YachayPeru.Application.Actions.Courses;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Retos.Queries.GetRetoVersions
{
    public record GetRetoVersionsQuery(int RetoId) : IRequest<IReadOnlyList<RetoVersionSummary>>;
    public record GetRetoVersionDetailQuery(int VersionId, int RetoId) : IRequest<Result<RetoVersionDetail>>;
}
