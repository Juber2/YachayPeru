using Microsoft.EntityFrameworkCore;
using YachayPeru.Application.Abstractions.Persistence.Aprendiz;
using YachayPeru.Domain.Entities.Aprendiz;
using YachayPeru.Infrastructure.Persistence.SqlServer;
using System.Linq.Expressions;

namespace YachayPeru.Infrastructure.Persistence.Repositories
{
    public class AprendizRegionExploredRepository : IAprendizRegionExploredRepository
    {
        private readonly ApplicationDbContext context;

        public AprendizRegionExploredRepository(ApplicationDbContext _context) => context = _context;

        public async Task<AprendizRegionExplored> AddAsync(AprendizRegionExplored entity, CancellationToken ct = default)
        {
            await context.AprendizRegionExplored.AddAsync(entity, ct);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<AprendizRegionExplored> entities, CancellationToken ct = default)
            => await context.AprendizRegionExplored.AddRangeAsync(entities, ct);

        public async Task<bool> AnyAsync(Expression<Func<AprendizRegionExplored, bool>> predicate, CancellationToken ct = default)
            => await context.AprendizRegionExplored.Where(x => !x.Deleted).AnyAsync(predicate, ct);

        public async Task<int> CountAsync(Expression<Func<AprendizRegionExplored, bool>>? predicate = null, CancellationToken ct = default)
        {
            var q = context.AprendizRegionExplored.Where(x => !x.Deleted);
            if (predicate is not null) q = q.Where(predicate);
            return await q.CountAsync(ct);
        }

        public void Delete(AprendizRegionExplored entity) => context.AprendizRegionExplored.Remove(entity);
        public void DeleteRange(IEnumerable<AprendizRegionExplored> entities) => context.AprendizRegionExplored.RemoveRange(entities);

        public async Task<AprendizRegionExplored?> FirstOrDefaultAsync(Expression<Func<AprendizRegionExplored, bool>> predicate, CancellationToken ct = default)
            => await context.AprendizRegionExplored.Where(x => !x.Deleted).FirstOrDefaultAsync(predicate, ct);

        public async Task<AprendizRegionExplored?> GetByIdAsync(object id, CancellationToken ct = default)
        {
            if (id is not int reid) return null;
            return await context.AprendizRegionExplored.FirstOrDefaultAsync(x => x.Id == reid && !x.Deleted, ct);
        }

        public async Task<IReadOnlyList<AprendizRegionExplored>> ListAsync(CancellationToken ct = default)
            => await context.AprendizRegionExplored.Where(x => !x.Deleted).ToListAsync(ct);

        public async Task<IReadOnlyList<AprendizRegionExplored>> ListAsync(Expression<Func<AprendizRegionExplored, bool>> predicate, CancellationToken ct = default)
            => await context.AprendizRegionExplored.Where(x => !x.Deleted).Where(predicate).ToListAsync(ct);

        public async Task<AprendizRegionExplored?> SingleOrDefaultAsync(Expression<Func<AprendizRegionExplored, bool>> predicate, CancellationToken ct = default)
            => await context.AprendizRegionExplored.Where(x => !x.Deleted).SingleOrDefaultAsync(predicate, ct);

        public void Update(AprendizRegionExplored entity) => context.AprendizRegionExplored.Update(entity);

        public async Task<bool> HasExploredAsync(int userId, int regionId, CancellationToken ct = default)
            => await context.AprendizRegionExplored.AnyAsync(x => x.UserId == userId && x.RegionId == regionId && !x.Deleted, ct);

        public async Task<IReadOnlyList<int>> GetExploredRegionIdsAsync(int userId, CancellationToken ct = default)
            => await context.AprendizRegionExplored.Where(x => x.UserId == userId && !x.Deleted).Select(x => x.RegionId).ToListAsync(ct);
    }
}
