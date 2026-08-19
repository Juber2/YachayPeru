using MediatR;
using YachayPeru.Application.Actions.Roles;
using YachayPeru.Application.Actions.Roles.Models;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Administration.Roles.Commands.EditRole
{
    public class EditRoleHandler : IRequestHandler<EditRoleCommand, Result<int>>
    {
        private readonly PlatformRoleCrudActions roleCrudActions;

        public EditRoleHandler(PlatformRoleCrudActions _roleCrudActions)
        {
            roleCrudActions = _roleCrudActions;
        }

        public async Task<Result<int>> Handle(EditRoleCommand request, CancellationToken cancellationToken)
        {
            var input = new UpdatePlatformRoleInput
            {
                Id = request.Id,
                Name = request.Name,
                RoleCode = request.RoleCode,
                Description = request.Description,
                PermissionIds = request.PermissionIds
            };

            return await roleCrudActions.UpdateRole(input, cancellationToken);
        }
    }
}
