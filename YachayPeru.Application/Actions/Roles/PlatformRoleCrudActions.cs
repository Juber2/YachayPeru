using YachayPeru.Application.Abstractions.Persistence;
using YachayPeru.Application.Abstractions.Persistence.Access;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Application.Actions.Roles.Models;
using YachayPeru.Application.Common.Results;
using static YachayPeru.Application.Common.Results.ResultCodes;
using YachayPeru.Domain.Entities.Access;

namespace YachayPeru.Application.Actions.Roles
{
    public class PlatformRoleCrudActions
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IPlatformRoleRepository roleRepository;
        private readonly IPermissionRepository permissionRepository;
        private readonly ICurrentUser currentUser;

        public PlatformRoleCrudActions(
            IPlatformRoleRepository _roleRepository,
            IPermissionRepository _permissionRepository,
            IUnitOfWork _unitOfWork,
            ICurrentUser _currentUser)
        {
            roleRepository = _roleRepository;
            permissionRepository = _permissionRepository;
            unitOfWork = _unitOfWork;
            currentUser = _currentUser;
        }

        public async Task<Result<int>> CreateRole(CreatePlatformRoleInput input, CancellationToken ct)
        {
            var codeExists = await roleRepository.AnyAsync(x => x.RoleCode == input.RoleCode, ct);
            if (codeExists)
                return Result<int>.Failure("El código de rol ya está en uso.", Conflict);

            if (input.PermissionIds.Any())
            {
                var allExist = await permissionRepository.AllExistAsync(input.PermissionIds, ct);
                if (!allExist)
                    return Result<int>.Failure("Uno o más permisos no son válidos.", Validation);
            }

            var role = new PlatformRole
            {
                Name = input.Name,
                RoleCode = input.RoleCode,
                Description = input.Description,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = currentUser.Id
            };
            await roleRepository.AddAsync(role, ct);
            await unitOfWork.SaveChangesAsync(ct);

            if (input.PermissionIds.Any())
            {
                var permissions = input.PermissionIds.Select(pid => new PlatformRolePermission
                {
                    PlatformRoleId = role.Id,
                    PermissionId = pid,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = currentUser.Id
                });
                await roleRepository.AddPermissionsAsync(permissions, ct);
                await unitOfWork.SaveChangesAsync(ct);
            }

            return Result<int>.Success(role.Id);
        }

        public async Task<Result<int>> UpdateRole(UpdatePlatformRoleInput input, CancellationToken ct)
        {
            var role = await roleRepository.GetByIdAsync(input.Id, ct);
            if (role is null)
                return Result<int>.Failure("Rol no encontrado.", NotFound);

            var codeExists = await roleRepository.AnyAsync(x => x.RoleCode == input.RoleCode && x.Id != input.Id, ct);
            if (codeExists)
                return Result<int>.Failure("El código de rol ya está en uso.", Conflict);

            if (input.PermissionIds.Any())
            {
                var allExist = await permissionRepository.AllExistAsync(input.PermissionIds, ct);
                if (!allExist)
                    return Result<int>.Failure("Uno o más permisos no son válidos.", Validation);
            }

            role.Name = input.Name;
            role.RoleCode = input.RoleCode;
            role.Description = input.Description;
            role.UpdatedAt = DateTime.UtcNow;
            role.UpdatedBy = currentUser.Id;
            roleRepository.Update(role);

            // hard delete existing permissions — unique index prevents soft-delete + re-insert
            await roleRepository.DeletePermissionsAsync(input.Id, ct);
            await unitOfWork.SaveChangesAsync(ct);

            if (input.PermissionIds.Any())
            {
                var permissions = input.PermissionIds.Select(pid => new PlatformRolePermission
                {
                    PlatformRoleId = role.Id,
                    PermissionId = pid,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = currentUser.Id
                });
                await roleRepository.AddPermissionsAsync(permissions, ct);
            }

            await unitOfWork.SaveChangesAsync(ct);
            return Result<int>.Success(role.Id);
        }

        public async Task<Result> DeleteRole(int roleId, CancellationToken ct)
        {
            var role = await roleRepository.GetByIdAsync(roleId, ct);
            if (role is null)
                return Result.Failure("Rol no encontrado.", NotFound);

            var userCount = await roleRepository.CountUsersAssignedAsync(roleId, ct);
            if (userCount > 0)
                return Result.Failure(
                    $"No se puede eliminar el rol porque tiene {userCount} usuario{(userCount == 1 ? "" : "s")} asignado{(userCount == 1 ? "" : "s")}. Reasígnalos antes de eliminar.",
                    Conflict);

            await roleRepository.DeletePermissionsAsync(roleId, ct);

            role.Deleted = true;
            role.UpdatedAt = DateTime.UtcNow;
            role.UpdatedBy = currentUser.Id;
            roleRepository.Update(role);

            await unitOfWork.SaveChangesAsync(ct);
            return Result.Success();
        }
    }
}
