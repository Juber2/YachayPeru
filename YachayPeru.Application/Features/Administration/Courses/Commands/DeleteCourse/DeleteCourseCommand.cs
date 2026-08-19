using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Administration.Courses.Commands.DeleteCourse
{
    public record DeleteCourseCommand(int Id) : IRequest<Result>;
}
