using MediatR;

namespace YachayPeru.Application.Features.Administration.Courses.Queries.GetCourses
{
    public record GetCoursesQuery : IRequest<IReadOnlyList<CourseListItem>>;
}
