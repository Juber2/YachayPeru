using Microsoft.EntityFrameworkCore;
using YachayPeru.Application.Abstractions.Persistence.Access;
using YachayPeru.Domain.Entities.Access;
using YachayPeru.Infrastructure.Persistence.SqlServer;

namespace YachayPeru.Infrastructure.Persistence.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly ApplicationDbContext context;

        public PermissionRepository(ApplicationDbContext _context)
        {
            context = _context;
        }

        public async Task<IReadOnlyList<(Resource Resource, Permission Permission, string ActionName)>> GetMatrixAsync(
            CancellationToken ct = default)
        {
            var rows = await (
                from p in context.Permissions
                where !p.Deleted
                join r in context.Resources on p.ResourceId equals r.Id
                join mc in context.MasterCodes on p.PermissionCode equals mc.Code
                orderby r.Name, mc.OrderIndex
                select new { Resource = r, Permission = p, ActionName = mc.Name }
            ).ToListAsync(ct);

            return rows
                .Select(x => (x.Resource, x.Permission, x.ActionName))
                .ToList();
        }

        public async Task<IReadOnlyList<(Resource Resource, Permission Permission, string ActionName)>> GetByScopeAsync(
            string scope, CancellationToken ct = default)
        {
            var rows = await (
                from p in context.Permissions
                where !p.Deleted
                join r in context.Resources on p.ResourceId equals r.Id
                where r.Scope == scope
                join mc in context.MasterCodes on p.PermissionCode equals mc.Code
                orderby r.Name, mc.OrderIndex
                select new { Resource = r, Permission = p, ActionName = mc.Name }
            ).ToListAsync(ct);

            return rows
                .Select(x => (x.Resource, x.Permission, x.ActionName))
                .ToList();
        }

        public async Task<bool> AllExistAsync(IEnumerable<int> ids, CancellationToken ct = default)
        {
            var idList = ids.Distinct().ToList();
            var count = await context.Permissions
                .Where(p => idList.Contains(p.Id) && !p.Deleted)
                .CountAsync(ct);

            return count == idList.Count;
        }

        public async Task<IReadOnlyList<int>> GetIdsByResourceCodesAsync(IEnumerable<string> resourceCodes, CancellationToken ct = default)
        {
            var codes = resourceCodes.ToList();
            return await (
                from p in context.Permissions
                where !p.Deleted
                join r in context.Resources on p.ResourceId equals r.Id
                where codes.Contains(r.Code)
                select p.Id
            ).ToListAsync(ct);
        }
    }
}
