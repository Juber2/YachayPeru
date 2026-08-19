using Microsoft.EntityFrameworkCore;
using YachayPeru.Application.Abstractions.Persistence.Access;
using YachayPeru.Domain.Entities.Access;
using YachayPeru.Infrastructure.Persistence.SqlServer;
using System.Linq.Expressions;

namespace YachayPeru.Infrastructure.Persistence.Repositories
{
    public class PlatformRoleRepository : IPlatformRoleRepository
    {
        private readonly ApplicationDbContext context;

        public PlatformRoleRepository(ApplicationDbContext _context)
        {
            context = _context;
        }

        public async Task<bool> AnyAsync(Expression<Func<PlatformRole, bool>> predicate, CancellationToken ct = default)
        {
            return await context.PlatformRoles
                .Where(x => !x.Deleted)
                .AnyAsync(predicate, ct);
        }

        public async Task<IReadOnlyList<PlatformRole>> GetAllAsync(CancellationToken ct = default)
        {
            return await context.PlatformRoles
                .Where(x => !x.Deleted)
                .OrderBy(x => x.Name)
                .ToListAsync(ct);
        }

        public async Task<PlatformRole?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await context.PlatformRoles
                .FirstOrDefaultAsync(x => x.Id == id && !x.Deleted, ct);
        }

        public async Task<PlatformRole?> GetByCodeAsync(string roleCode, CancellationToken ct = default)
        {
            return await context.PlatformRoles
                .FirstOrDefaultAsync(x => x.RoleCode == roleCode && !x.Deleted, ct);
        }

        public async Task AddAsync(PlatformRole role, CancellationToken ct = default)
        {
            await context.PlatformRoles.AddAsync(role, ct);
        }

        public void Update(PlatformRole role)
        {
            context.PlatformRoles.Update(role);
        }

        public async Task<IReadOnlyList<PlatformRoleListRow>> GetListWithDetailsAsync(CancellationToken ct = default)
        {
            var rows = await (
                from role in context.PlatformRoles
                where !role.Deleted
                let userCount = context.Users.Count(u => u.RoleId == role.Id && !u.Deleted)
                join rp in context.PlatformRolePermissions.Where(x => !x.Deleted)
                    on role.Id equals rp.PlatformRoleId into rolePerms
                from rp in rolePerms.DefaultIfEmpty()
                join p in context.Permissions.Where(x => !x.Deleted)
                    on rp.PermissionId equals p.Id into perms
                from p in perms.DefaultIfEmpty()
                join r in context.Resources
                    on p.ResourceId equals r.Id into resources
                from r in resources.DefaultIfEmpty()
                join mc in context.MasterCodes
                    on p.PermissionCode equals mc.Code into codes
                from mc in codes.DefaultIfEmpty()
                orderby role.Name, r.Name, mc.OrderIndex
                select new PlatformRoleListRow(
                    role.Id,
                    role.Name,
                    role.RoleCode,
                    role.Description,
                    userCount,
                    p != null ? p.Id : (int?)null,
                    r != null ? r.Name : null,
                    mc != null ? mc.Value : null)
            ).ToListAsync(ct);

            return rows;
        }

        public async Task<IReadOnlyList<PlatformRolePermission>> GetPermissionsAsync(int roleId, CancellationToken ct = default)
        {
            return await context.PlatformRolePermissions
                .Where(x => x.PlatformRoleId == roleId && !x.Deleted)
                .ToListAsync(ct);
        }

        public async Task AddPermissionsAsync(IEnumerable<PlatformRolePermission> permissions, CancellationToken ct = default)
        {
            await context.PlatformRolePermissions.AddRangeAsync(permissions, ct);
        }

        public async Task DeletePermissionsAsync(int roleId, CancellationToken ct = default)
        {
            var existing = await context.PlatformRolePermissions
                .Where(x => x.PlatformRoleId == roleId)
                .ToListAsync(ct);

            context.PlatformRolePermissions.RemoveRange(existing);
        }

        public async Task<int> CountUsersAssignedAsync(int roleId, CancellationToken ct = default)
        {
            return await context.Users
                .Where(x => x.RoleId == roleId && !x.Deleted)
                .CountAsync(ct);
        }
    }
}
