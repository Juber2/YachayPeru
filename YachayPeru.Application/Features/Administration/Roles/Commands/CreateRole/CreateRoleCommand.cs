using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Administration.Roles.Commands.CreateRole
{
    public sealed record CreateRoleCommand : IRequest<Result<int>>
    {
        public string Name { get; init; } = default!;
        public string RoleCode { get; init; } = default!;
        public string? Description { get; init; }
        public IReadOnlyCollection<int> PermissionIds { get; init; } = [];
    }
}
