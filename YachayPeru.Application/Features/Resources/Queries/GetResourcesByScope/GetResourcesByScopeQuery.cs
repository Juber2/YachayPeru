using MediatR;

namespace YachayPeru.Application.Features.Resources.Queries.GetResourcesByScope
{
    public record GetResourcesByScopeQuery(string Scope)
        : IRequest<IReadOnlyList<ResourcePermissionItem>>;
}
