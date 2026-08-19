using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Administration.Courses.Commands.CreateCourse
{
    public sealed record CreateCourseCommand : IRequest<Result<int>>
    {
        public string Title { get; init; } = default!;
        public string? Description { get; init; }
        public string? ZoneCode { get; init; }
    }
}
