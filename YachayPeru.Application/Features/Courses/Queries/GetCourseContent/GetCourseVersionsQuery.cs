using MediatR;
using YachayPeru.Application.Actions.Courses;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Courses.Queries.GetCourseContent
{
    public record GetCourseVersionsQuery(int CourseId) : IRequest<IReadOnlyList<VersionSummary>>;
    public record GetCourseVersionDetailQuery(int VersionId, int CourseId) : IRequest<Result<VersionDetail>>;
}
