using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Courses.Queries.GetCourseContent
{
    public record GetCourseContentQuery(int CourseId) : IRequest<Result<CourseContentDetail>>;
}
