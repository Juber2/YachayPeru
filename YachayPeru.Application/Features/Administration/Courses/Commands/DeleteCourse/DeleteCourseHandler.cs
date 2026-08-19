using MediatR;
using YachayPeru.Application.Actions.Courses;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Administration.Courses.Commands.DeleteCourse
{
    public class DeleteCourseHandler : IRequestHandler<DeleteCourseCommand, Result>
    {
        private readonly CourseCrudActions courseActions;

        public DeleteCourseHandler(CourseCrudActions _courseActions) => courseActions = _courseActions;

        public Task<Result> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
            => courseActions.DeleteCourse(request.Id, cancellationToken);
    }
}
