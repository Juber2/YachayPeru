using MediatR;
using YachayPeru.Application.Actions.Roles;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Administration.Roles.Commands.DeleteRole
{
    public class DeleteRoleHandler : IRequestHandler<DeleteRoleCommand, Result>
    {
        private readonly PlatformRoleCrudActions roleCrudActions;

        public DeleteRoleHandler(PlatformRoleCrudActions _roleCrudActions)
        {
            roleCrudActions = _roleCrudActions;
        }

        public async Task<Result> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            return await roleCrudActions.DeleteRole(request.Id, cancellationToken);
        }
    }
}
