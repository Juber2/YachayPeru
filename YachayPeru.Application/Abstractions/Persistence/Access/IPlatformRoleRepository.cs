using System.Linq.Expressions;
using YachayPeru.Domain.Entities.Access;

namespace YachayPeru.Application.Abstractions.Persistence.Access
{
    public record PlatformRoleListRow(
        int     RoleId,
        string  RoleName,
        string? RoleCode,
        string? Description,
        int     UserCount,
        int?    PermissionId,
        string? ResourceName,
        string? ActionValue);

    public interface IPlatformRoleRepository
    {
        Task<bool> AnyAsync(Expression<Func<PlatformRole, bool>> predicate, CancellationToken ct = default);
        Task<IReadOnlyList<PlatformRole>> GetAllAsync(CancellationToken ct = default);
        Task<PlatformRole?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<PlatformRole?> GetByCodeAsync(string roleCode, CancellationToken ct = default);
        Task AddAsync(PlatformRole role, CancellationToken ct = default);
        void Update(PlatformRole role);
        Task<IReadOnlyList<PlatformRolePermission>> GetPermissionsAsync(int roleId, CancellationToken ct = default);
        Task<IReadOnlyList<PlatformRoleListRow>> GetListWithDetailsAsync(CancellationToken ct = default);
        Task AddPermissionsAsync(IEnumerable<PlatformRolePermission> permissions, CancellationToken ct = default);
        Task DeletePermissionsAsync(int roleId, CancellationToken ct = default);
        Task<int> CountUsersAssignedAsync(int roleId, CancellationToken ct = default);
    }
}
