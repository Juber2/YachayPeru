using MediatR;
using YachayPeru.Application.Abstractions.Persistence.Access;

namespace YachayPeru.Application.Features.Resources.Queries.GetResourcesByScope
{
    public sealed class GetResourcesByScopeHandler
        : IRequestHandler<GetResourcesByScopeQuery, IReadOnlyList<ResourcePermissionItem>>
    {
        private readonly IPermissionRepository _permissionRepository;

        public GetResourcesByScopeHandler(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task<IReadOnlyList<ResourcePermissionItem>> Handle(
            GetResourcesByScopeQuery request, CancellationToken ct)
        {
            var rows = await _permissionRepository.GetByScopeAsync(request.Scope, ct);

            return rows
                .GroupBy(x => new { x.Resource.Code, x.Resource.Name })
                .Select(g => new ResourcePermissionItem(
                    g.Key.Code,
                    g.Key.Name,
                    g.Select(x => new PermissionItem(x.Permission.Id, x.Permission.PermissionCode)).ToList()
                ))
                .ToList();
        }
    }
}
