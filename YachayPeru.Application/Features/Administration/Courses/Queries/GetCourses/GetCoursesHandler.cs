using MediatR;
using YachayPeru.Application.Abstractions.Persistence.Learning;

namespace YachayPeru.Application.Features.Administration.Courses.Queries.GetCourses
{
    public class GetCoursesHandler : IRequestHandler<GetCoursesQuery, IReadOnlyList<CourseListItem>>
    {
        private readonly ICourseRepository courseRepository;

        public GetCoursesHandler(ICourseRepository _courseRepository) => courseRepository = _courseRepository;

        public async Task<IReadOnlyList<CourseListItem>> Handle(GetCoursesQuery request, CancellationToken cancellationToken)
        {
            var courses = await courseRepository.ListAsync(cancellationToken);
            return courses.Select(c => new CourseListItem
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                IsActive = c.IsActive,
                CoverImageUrl = c.CoverImageUrl,
                CreatedAt = c.CreatedAt
            }).ToList();
        }
    }
}
