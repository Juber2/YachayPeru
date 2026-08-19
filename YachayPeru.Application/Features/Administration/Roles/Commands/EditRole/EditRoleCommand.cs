using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Administration.Roles.Commands.EditRole
{
    public sealed record EditRoleCommand : IRequest<Result<int>>
    {
        public int Id { get; init; }
        public string Name { get; init; } = default!;
        public string RoleCode { get; init; } = default!;
        public string? Description { get; init; }
        public IReadOnlyCollection<int> PermissionIds { get; init; } = [];
    }
}
