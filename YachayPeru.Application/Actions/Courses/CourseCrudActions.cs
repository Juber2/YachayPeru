using YachayPeru.Application.Abstractions.Persistence;
using YachayPeru.Application.Abstractions.Persistence.Learning;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Application.Actions.Courses.Models;
using YachayPeru.Application.Common.Results;
using YachayPeru.Domain.Constants;
using YachayPeru.Domain.Entities.Learning;
using static YachayPeru.Application.Common.Results.ResultCodes;

namespace YachayPeru.Application.Actions.Courses
{
    public class CourseCrudActions
    {
        private readonly ICourseRepository courseRepository;
        private readonly ICourseVersionRepository courseVersionRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUser currentUser;

        public CourseCrudActions(
            ICourseRepository _courseRepository,
            ICourseVersionRepository _courseVersionRepository,
            IUnitOfWork _unitOfWork,
            ICurrentUser _currentUser)
        {
            courseRepository = _courseRepository;
            courseVersionRepository = _courseVersionRepository;
            unitOfWork = _unitOfWork;
            currentUser = _currentUser;
        }

        public async Task<Result<int>> CreateCourse(CreateCourseInput input, CancellationToken ct)
        {
            var course = new Course
            {
                Title = input.Title,
                Description = input.Description,
                ZoneCode = input.ZoneCode,
                AmbientAudioTitle = input.AmbientAudioTitle,
                SpotifyUrl = input.SpotifyUrl,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = currentUser.Id
            };

            await courseRepository.AddAsync(course, ct);
            await unitOfWork.SaveChangesAsync(ct);

            var version = new CourseVersion
            {
                CourseId = course.Id,
                VersionNumber = 1,
                StatusCode = AppConstants.CourseVersionStatus.Draft,
                IsCurrent = false,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = currentUser.Id
            };

            await courseVersionRepository.AddAsync(version, ct);
            await unitOfWork.SaveChangesAsync(ct);

            return Result<int>.Success(course.Id);
        }

        public async Task<Result<int>> UpdateCourseInfo(UpdateCourseInfoInput input, CancellationToken ct)
        {
            var course = await courseRepository.GetByIdAsync(input.Id, ct);
            if (course is null)
                return Result<int>.Failure("Curso no encontrado.", NotFound);

            course.Title = input.Title;
            course.Description = input.Description;
            course.IsActive = input.IsActive;
            course.ZoneCode = input.ZoneCode;
            course.AmbientAudioTitle = input.AmbientAudioTitle;
            course.SpotifyUrl = input.SpotifyUrl;
            course.UpdatedAt = DateTime.UtcNow;
            course.UpdatedBy = currentUser.Id;

            courseRepository.Update(course);
            await unitOfWork.SaveChangesAsync(ct);

            return Result<int>.Success(course.Id);
        }

        public async Task<Result> DeleteCourse(int id, CancellationToken ct)
        {
            var course = await courseRepository.GetByIdAsync(id, ct);
            if (course is null)
                return Result.Failure("Curso no encontrado.", NotFound);

            course.Deleted = true;
            course.UpdatedAt = DateTime.UtcNow;
            course.UpdatedBy = currentUser.Id;

            courseRepository.Update(course);
            await unitOfWork.SaveChangesAsync(ct);

            return Result.Success();
        }
    }
}
