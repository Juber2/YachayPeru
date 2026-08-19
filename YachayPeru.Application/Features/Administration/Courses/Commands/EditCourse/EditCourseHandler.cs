using MediatR;
using YachayPeru.Application.Actions.Courses;
using YachayPeru.Application.Actions.Courses.Models;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Administration.Courses.Commands.EditCourse
{
    public class EditCourseHandler : IRequestHandler<EditCourseCommand, Result<int>>
    {
        private readonly CourseCrudActions courseActions;

        public EditCourseHandler(CourseCrudActions _courseActions) => courseActions = _courseActions;

        public Task<Result<int>> Handle(EditCourseCommand request, CancellationToken cancellationToken)
            => courseActions.UpdateCourseInfo(new UpdateCourseInfoInput
            {
                Id = request.Id,
                Title = request.Title,
                Description = request.Description,
                IsActive = request.IsActive,
                ZoneCode = request.ZoneCode
            }, cancellationToken);
    }
}
