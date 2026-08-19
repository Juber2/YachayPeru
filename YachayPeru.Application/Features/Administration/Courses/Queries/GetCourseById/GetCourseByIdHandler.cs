using MediatR;
using YachayPeru.Application.Abstractions.Persistence.Learning;
using YachayPeru.Application.Common.Results;
using static YachayPeru.Application.Common.Results.ResultCodes;

namespace YachayPeru.Application.Features.Administration.Courses.Queries.GetCourseById
{
    public class GetCourseByIdHandler : IRequestHandler<GetCourseByIdQuery, Result<CourseDetail>>
    {
        private readonly ICourseRepository courseRepository;

        public GetCourseByIdHandler(ICourseRepository _courseRepository) => courseRepository = _courseRepository;

        public async Task<Result<CourseDetail>> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
        {
            var course = await courseRepository.GetByIdAsync(request.Id, cancellationToken);
            if (course is null)
                return Result<CourseDetail>.Failure("Región no encontrada.", NotFound);

            return Result<CourseDetail>.Success(new CourseDetail
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                IsActive = course.IsActive,
                CoverImageUrl = course.CoverImageUrl,
                SourceTemplateId = course.SourceTemplateId,
                ZoneCode = course.ZoneCode,
                CreatedAt = course.CreatedAt
            });
        }
    }
}
