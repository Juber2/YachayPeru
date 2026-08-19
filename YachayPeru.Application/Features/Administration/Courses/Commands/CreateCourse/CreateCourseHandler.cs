using MediatR;
using YachayPeru.Application.Actions.Courses;
using YachayPeru.Application.Actions.Courses.Models;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Administration.Courses.Commands.CreateCourse
{
    public class CreateCourseHandler : IRequestHandler<CreateCourseCommand, Result<int>>
    {
        private readonly CourseCrudActions courseActions;

        public CreateCourseHandler(CourseCrudActions _courseActions) => courseActions = _courseActions;

        public Task<Result<int>> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
            => courseActions.CreateCourse(new CreateCourseInput
            {
                Title = request.Title,
                Description = request.Description,
                ZoneCode = request.ZoneCode
            }, cancellationToken);
    }
}
