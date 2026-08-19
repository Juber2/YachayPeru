using MediatR;
using YachayPeru.Application.Actions.Users;
using YachayPeru.Application.Actions.Users.Models;
using YachayPeru.Application.Common.Results;
using YachayPeru.Domain.Constants;

namespace YachayPeru.Application.Features.Administration.Users.Commands.CreateUser
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, Result<int>>
    {
        private readonly UserPlatformCrudActions userCrudActions;

        public CreateUserHandler(UserPlatformCrudActions _userCrudActions)
        {
            userCrudActions = _userCrudActions;
        }

        public async Task<Result<int>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var input = new CreateUserInput
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Username = request.UserName,
                Email = request.Email,
                Password = request.Password,
                UserTypeCode = AppConstants.UserType.Platform,
                RoleId = request.RoleId,
                ReactivateUserId = request.ReactivateUserId,
                SendWelcomeMessage = request.SendWelcomeMessage
            };

            return await userCrudActions.CreateUser(input, cancellationToken);
        }
    }
}
