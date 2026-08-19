using MediatR;
using YachayPeru.Application.Abstractions.Persistence.Access;
using YachayPeru.Application.Common.Results;
using static YachayPeru.Application.Common.Results.ResultCodes;

namespace YachayPeru.Application.Features.Administration.Roles.Queries.GetRoleDetail
{
    public sealed record GetRoleDetailQuery(int Id) : IRequest<Result<PlatformRoleDetailDto>>;

    public sealed record PlatformRoleDetailDto(
        int Id,
        string Name,
        string? RoleCode,
        string? Description,
        IReadOnlyList<int> PermissionIds);

    public class GetRoleDetailHandler : IRequestHandler<GetRoleDetailQuery, Result<PlatformRoleDetailDto>>
    {
        private readonly IPlatformRoleRepository roleRepository;

        public GetRoleDetailHandler(IPlatformRoleRepository _roleRepository)
        {
            roleRepository = _roleRepository;
        }

        public async Task<Result<PlatformRoleDetailDto>> Handle(
            GetRoleDetailQuery request, CancellationToken cancellationToken)
        {
            var role = await roleRepository.GetByIdAsync(request.Id, cancellationToken);
            if (role is null)
                return Result<PlatformRoleDetailDto>.Failure("Rol no encontrado.", NotFound);

            var permissions = await roleRepository.GetPermissionsAsync(request.Id, cancellationToken);
            var permissionIds = permissions.Select(p => p.PermissionId).ToList();

            var dto = new PlatformRoleDetailDto(
                role.Id, role.Name, role.RoleCode, role.Description, permissionIds);

            return Result<PlatformRoleDetailDto>.Success(dto);
        }
    }
}
