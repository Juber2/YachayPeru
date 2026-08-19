using Microsoft.EntityFrameworkCore;
using YachayPeru.Application.Abstractions.Persistence.Aprendiz;
using YachayPeru.Domain.Entities.Aprendiz;
using YachayPeru.Infrastructure.Persistence.SqlServer;
using System.Linq.Expressions;

namespace YachayPeru.Infrastructure.Persistence.Repositories
{
    public class AprendizRegionActivityRepository : IAprendizRegionActivityRepository
    {
        private readonly ApplicationDbContext context;

        public AprendizRegionActivityRepository(ApplicationDbContext _context) => context = _context;

        public async Task<AprendizRegionActivity> AddAsync(AprendizRegionActivity entity, CancellationToken ct = default)
        {
            await context.AprendizRegionActivities.AddAsync(entity, ct);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<AprendizRegionActivity> entities, CancellationToken ct = default)
            => await context.AprendizRegionActivities.AddRangeAsync(entities, ct);

        public async Task<bool> AnyAsync(Expression<Func<AprendizRegionActivity, bool>> predicate, CancellationToken ct = default)
            => await context.AprendizRegionActivities.Where(x => !x.Deleted).AnyAsync(predicate, ct);

        public async Task<int> CountAsync(Expression<Func<AprendizRegionActivity, bool>>? predicate = null, CancellationToken ct = default)
        {
            var q = context.AprendizRegionActivities.Where(x => !x.Deleted);
            if (predicate is not null) q = q.Where(predicate);
            return await q.CountAsync(ct);
        }

        public void Delete(AprendizRegionActivity entity) => context.AprendizRegionActivities.Remove(entity);
        public void DeleteRange(IEnumerable<AprendizRegionActivity> entities) => context.AprendizRegionActivities.RemoveRange(entities);

        public async Task<AprendizRegionActivity?> FirstOrDefaultAsync(Expression<Func<AprendizRegionActivity, bool>> predicate, CancellationToken ct = default)
            => await context.AprendizRegionActivities.Where(x => !x.Deleted).FirstOrDefaultAsync(predicate, ct);

        public async Task<AprendizRegionActivity?> GetByIdAsync(object id, CancellationToken ct = default)
        {
            if (id is not int raid) return null;
            return await context.AprendizRegionActivities.FirstOrDefaultAsync(x => x.Id == raid && !x.Deleted, ct);
        }

        public async Task<IReadOnlyList<AprendizRegionActivity>> ListAsync(CancellationToken ct = default)
            => await context.AprendizRegionActivities.Where(x => !x.Deleted).ToListAsync(ct);

        public async Task<IReadOnlyList<AprendizRegionActivity>> ListAsync(Expression<Func<AprendizRegionActivity, bool>> predicate, CancellationToken ct = default)
            => await context.AprendizRegionActivities.Where(x => !x.Deleted).Where(predicate).ToListAsync(ct);

        public async Task<AprendizRegionActivity?> SingleOrDefaultAsync(Expression<Func<AprendizRegionActivity, bool>> predicate, CancellationToken ct = default)
            => await context.AprendizRegionActivities.Where(x => !x.Deleted).SingleOrDefaultAsync(predicate, ct);

        public void Update(AprendizRegionActivity entity) => context.AprendizRegionActivities.Update(entity);

        public async Task<AprendizRegionActivity?> GetByUserIdAsync(int userId, CancellationToken ct = default)
            => await context.AprendizRegionActivities.FirstOrDefaultAsync(x => x.UserId == userId && !x.Deleted, ct);
    }
}
