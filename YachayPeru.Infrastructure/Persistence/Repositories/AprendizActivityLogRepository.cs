using Microsoft.EntityFrameworkCore;
using YachayPeru.Application.Abstractions.Persistence.Aprendiz;
using YachayPeru.Domain.Entities.Aprendiz;
using YachayPeru.Infrastructure.Persistence.SqlServer;
using System.Linq.Expressions;

namespace YachayPeru.Infrastructure.Persistence.Repositories
{
    public class AprendizActivityLogRepository : IAprendizActivityLogRepository
    {
        private readonly ApplicationDbContext context;

        public AprendizActivityLogRepository(ApplicationDbContext _context) => context = _context;

        public async Task<AprendizActivityLog> AddAsync(AprendizActivityLog entity, CancellationToken ct = default)
        {
            await context.AprendizActivityLogs.AddAsync(entity, ct);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<AprendizActivityLog> entities, CancellationToken ct = default)
            => await context.AprendizActivityLogs.AddRangeAsync(entities, ct);

        public async Task<bool> AnyAsync(Expression<Func<AprendizActivityLog, bool>> predicate, CancellationToken ct = default)
            => await context.AprendizActivityLogs.Where(x => !x.Deleted).AnyAsync(predicate, ct);

        public async Task<int> CountAsync(Expression<Func<AprendizActivityLog, bool>>? predicate = null, CancellationToken ct = default)
        {
            var q = context.AprendizActivityLogs.Where(x => !x.Deleted);
            if (predicate is not null) q = q.Where(predicate);
            return await q.CountAsync(ct);
        }

        public void Delete(AprendizActivityLog entity) => context.AprendizActivityLogs.Remove(entity);
        public void DeleteRange(IEnumerable<AprendizActivityLog> entities) => context.AprendizActivityLogs.RemoveRange(entities);

        public async Task<AprendizActivityLog?> FirstOrDefaultAsync(Expression<Func<AprendizActivityLog, bool>> predicate, CancellationToken ct = default)
            => await context.AprendizActivityLogs.Where(x => !x.Deleted).FirstOrDefaultAsync(predicate, ct);

        public async Task<AprendizActivityLog?> GetByIdAsync(object id, CancellationToken ct = default)
        {
            if (id is not int aid) return null;
            return await context.AprendizActivityLogs.FirstOrDefaultAsync(x => x.Id == aid && !x.Deleted, ct);
        }

        public async Task<IReadOnlyList<AprendizActivityLog>> ListAsync(CancellationToken ct = default)
            => await context.AprendizActivityLogs.Where(x => !x.Deleted).ToListAsync(ct);

        public async Task<IReadOnlyList<AprendizActivityLog>> ListAsync(Expression<Func<AprendizActivityLog, bool>> predicate, CancellationToken ct = default)
            => await context.AprendizActivityLogs.Where(x => !x.Deleted).Where(predicate).ToListAsync(ct);

        public async Task<AprendizActivityLog?> SingleOrDefaultAsync(Expression<Func<AprendizActivityLog, bool>> predicate, CancellationToken ct = default)
            => await context.AprendizActivityLogs.Where(x => !x.Deleted).SingleOrDefaultAsync(predicate, ct);

        public void Update(AprendizActivityLog entity) => context.AprendizActivityLogs.Update(entity);

        public async Task<IReadOnlyList<AprendizActivityLog>> GetByUserAsync(int userId, CancellationToken ct = default)
            => await context.AprendizActivityLogs
                .Where(x => x.UserId == userId && !x.Deleted)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync(ct);

        public async Task<AprendizActivityLog?> GetLastWithRegionAsync(int userId, CancellationToken ct = default)
            => await context.AprendizActivityLogs
                .Where(x => x.UserId == userId && !x.Deleted && x.RegionId != null)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync(ct);
    }
}
