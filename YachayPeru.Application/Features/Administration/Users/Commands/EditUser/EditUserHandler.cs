using MediatR;
using YachayPeru.Application.Actions.Users;
using YachayPeru.Application.Actions.Users.Models;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Administration.Users.Commands.EditUser
{
    public class EditUserHandler : IRequestHandler<EditUserCommand, Result<int>>
    {
        private readonly UserPlatformCrudActions userCrudActions;

        public EditUserHandler(UserPlatformCrudActions _userCrudActions)
        {
            userCrudActions = _userCrudActions;
        }

        public async Task<Result<int>> Handle(EditUserCommand request, CancellationToken cancellationToken)
        {
            var input = new UpdateUserInput
            {
                Id = request.Id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Username = request.Username,
                Email = request.Email,
                Password = request.Password,
                IsLocked = request.IsLocked,
                RoleId = request.RoleId
            };

            return await userCrudActions.UpdateUser(input, cancellationToken);
        }
    }
}
