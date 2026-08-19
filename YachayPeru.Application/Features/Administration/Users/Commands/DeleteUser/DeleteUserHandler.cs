using MediatR;
using YachayPeru.Application.Actions.Users;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Administration.Users.Commands.DeleteUser
{
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, Result>
    {
        private readonly UserPlatformCrudActions userCrudActions;

        public DeleteUserHandler(UserPlatformCrudActions _userCrudActions)
        {
            userCrudActions = _userCrudActions;
        }

        public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            return await userCrudActions.DeleteUser(request.Id, cancellationToken);
        }
    }
}
