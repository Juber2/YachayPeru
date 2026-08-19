using MediatR;
using YachayPeru.Application.Abstractions.Persistence.Access;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Administration.Roles.Queries.GetRoleList
{
    public sealed record GetRoleListQuery : IRequest<Result<IReadOnlyList<PlatformRoleListDto>>>;

    public sealed record PlatformRolePermissionDto(string ResourceName, string ActionValue);

    public sealed record PlatformRoleListDto(
        int     Id,
        string  Name,
        string? RoleCode,
        string? Description,
        int     UserCount,
        IReadOnlyList<PlatformRolePermissionDto> Permissions);

    public class GetRoleListHandler : IRequestHandler<GetRoleListQuery, Result<IReadOnlyList<PlatformRoleListDto>>>
    {
        private readonly IPlatformRoleRepository _roleRepository;

        public GetRoleListHandler(IPlatformRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<Result<IReadOnlyList<PlatformRoleListDto>>> Handle(
            GetRoleListQuery request, CancellationToken ct)
        {
            var rows = await _roleRepository.GetListWithDetailsAsync(ct);

            var dtos = rows
                .GroupBy(r => new { r.RoleId, r.RoleName, r.RoleCode, r.Description, r.UserCount })
                .Select(g => new PlatformRoleListDto(
                    g.Key.RoleId,
                    g.Key.RoleName,
                    g.Key.RoleCode,
                    g.Key.Description,
                    g.Key.UserCount,
                    g.Where(x => x.PermissionId.HasValue && x.ResourceName != null)
                     .Select(x => new PlatformRolePermissionDto(x.ResourceName!, x.ActionValue ?? ""))
                     .ToList()
                ))
                .ToList();

            return Result<IReadOnlyList<PlatformRoleListDto>>.Success(dtos);
        }
    }
}
