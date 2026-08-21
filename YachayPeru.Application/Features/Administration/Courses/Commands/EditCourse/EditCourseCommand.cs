using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Administration.Courses.Commands.EditCourse
{
    public sealed record EditCourseCommand : IRequest<Result<int>>
    {
        public int Id { get; init; }
        public string Title { get; init; } = default!;
        public string? Description { get; init; }
        public bool IsActive { get; init; }
        public string? ZoneCode { get; init; }
        public string? AmbientAudioTitle { get; init; }
        public string? SpotifyUrl { get; init; }
    }
}
