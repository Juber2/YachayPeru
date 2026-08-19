using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Administration.Roles.Commands.DeleteRole
{
    public sealed record DeleteRoleCommand(int Id) : IRequest<Result>;
}
