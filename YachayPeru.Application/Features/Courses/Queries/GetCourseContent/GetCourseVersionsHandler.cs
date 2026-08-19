using MediatR;
using YachayPeru.Application.Actions.Courses;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Courses.Queries.GetCourseContent
{
    public class GetCourseVersionsHandler : IRequestHandler<GetCourseVersionsQuery, IReadOnlyList<VersionSummary>>
    {
        private readonly CourseContentActions contentActions;
        public GetCourseVersionsHandler(CourseContentActions _contentActions) => contentActions = _contentActions;
        public Task<IReadOnlyList<VersionSummary>> Handle(GetCourseVersionsQuery request, CancellationToken ct)
            => contentActions.GetVersionHistory(request.CourseId, ct);
    }

    public class GetCourseVersionDetailHandler : IRequestHandler<GetCourseVersionDetailQuery, Result<VersionDetail>>
    {
        private readonly CourseContentActions contentActions;
        public GetCourseVersionDetailHandler(CourseContentActions _contentActions) => contentActions = _contentActions;
        public Task<Result<VersionDetail>> Handle(GetCourseVersionDetailQuery request, CancellationToken ct)
            => contentActions.GetVersionDetail(request.VersionId, request.CourseId, ct);
    }
}
