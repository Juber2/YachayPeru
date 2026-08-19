using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Administration.Courses.Queries.GetCourseById
{
    public record GetCourseByIdQuery(int Id) : IRequest<Result<CourseDetail>>;
}
