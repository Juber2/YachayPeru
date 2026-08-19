using MediatR;
using YachayPeru.Application.Actions.Roles;
using YachayPeru.Application.Actions.Roles.Models;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Administration.Roles.Commands.CreateRole
{
    public class CreateRoleHandler : IRequestHandler<CreateRoleCommand, Result<int>>
    {
        private readonly PlatformRoleCrudActions roleCrudActions;

        public CreateRoleHandler(PlatformRoleCrudActions _roleCrudActions)
        {
            roleCrudActions = _roleCrudActions;
        }

        public async Task<Result<int>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var input = new CreatePlatformRoleInput
            {
                Name = request.Name,
                RoleCode = request.RoleCode,
                Description = request.Description,
                PermissionIds = request.PermissionIds
            };

            return await roleCrudActions.CreateRole(input, cancellationToken);
        }
    }
}
