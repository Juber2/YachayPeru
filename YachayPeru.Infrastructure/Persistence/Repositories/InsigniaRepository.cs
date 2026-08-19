using Microsoft.EntityFrameworkCore;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Domain.Entities.Content;
using YachayPeru.Infrastructure.Persistence.SqlServer;
using System.Linq.Expressions;

namespace YachayPeru.Infrastructure.Persistence.Repositories
{
    public class InsigniaRepository : IInsigniaRepository
    {
        private readonly ApplicationDbContext context;

        public InsigniaRepository(ApplicationDbContext _context) => context = _context;

        public async Task<Insignia> AddAsync(Insignia entity, CancellationToken ct = default)
        {
            await context.Insignias.AddAsync(entity, ct);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<Insignia> entities, CancellationToken ct = default)
            => await context.Insignias.AddRangeAsync(entities, ct);

        public async Task<bool> AnyAsync(Expression<Func<Insignia, bool>> predicate, CancellationToken ct = default)
            => await context.Insignias.Where(x => !x.Deleted).AnyAsync(predicate, ct);

        public async Task<int> CountAsync(Expression<Func<Insignia, bool>>? predicate = null, CancellationToken ct = default)
        {
            var q = context.Insignias.Where(x => !x.Deleted);
            if (predicate is not null) q = q.Where(predicate);
            return await q.CountAsync(ct);
        }

        public void Delete(Insignia entity) => context.Insignias.Remove(entity);
        public void DeleteRange(IEnumerable<Insignia> entities) => context.Insignias.RemoveRange(entities);

        public async Task<Insignia?> FirstOrDefaultAsync(Expression<Func<Insignia, bool>> predicate, CancellationToken ct = default)
            => await context.Insignias.Where(x => !x.Deleted).FirstOrDefaultAsync(predicate, ct);

        public async Task<Insignia?> GetByIdAsync(object id, CancellationToken ct = default)
        {
            if (id is not int iid) return null;
            return await context.Insignias.FirstOrDefaultAsync(x => x.Id == iid && !x.Deleted, ct);
        }

        public async Task<IReadOnlyList<Insignia>> ListAsync(CancellationToken ct = default)
            => await context.Insignias.Where(x => !x.Deleted).ToListAsync(ct);

        public async Task<IReadOnlyList<Insignia>> ListAsync(Expression<Func<Insignia, bool>> predicate, CancellationToken ct = default)
            => await context.Insignias.Where(x => !x.Deleted).Where(predicate).ToListAsync(ct);

        public async Task<Insignia?> SingleOrDefaultAsync(Expression<Func<Insignia, bool>> predicate, CancellationToken ct = default)
            => await context.Insignias.Where(x => !x.Deleted).SingleOrDefaultAsync(predicate, ct);

        public void Update(Insignia entity) => context.Insignias.Update(entity);

        public async Task<IReadOnlyList<int>> GetRequiredRegionIdsAsync(int insigniaId, CancellationToken ct = default)
            => await context.InsigniaRequiredRegions
                .Where(x => x.InsigniaId == insigniaId && !x.Deleted)
                .Select(x => x.CourseId)
                .ToListAsync(ct);

        public async Task<IReadOnlyList<int>> GetRequiredRetoIdsAsync(int insigniaId, CancellationToken ct = default)
            => await context.InsigniaRequiredRetos
                .Where(x => x.InsigniaId == insigniaId && !x.Deleted)
                .Select(x => x.RetoId)
                .ToListAsync(ct);

        public async Task ReplaceRequiredRegionsAsync(int insigniaId, IReadOnlyCollection<int> regionIds, CancellationToken ct = default)
        {
            var existing = await context.InsigniaRequiredRegions
                .Where(x => x.InsigniaId == insigniaId)
                .ToListAsync(ct);
            context.InsigniaRequiredRegions.RemoveRange(existing);

            var newRows = regionIds.Select(regionId => new InsigniaRequiredRegion
            {
                InsigniaId = insigniaId,
                CourseId = regionId,
                CreatedAt = DateTime.UtcNow
            });
            await context.InsigniaRequiredRegions.AddRangeAsync(newRows, ct);
        }

        public async Task ReplaceRequiredRetosAsync(int insigniaId, IReadOnlyCollection<int> retoIds, CancellationToken ct = default)
        {
            var existing = await context.InsigniaRequiredRetos
                .Where(x => x.InsigniaId == insigniaId)
                .ToListAsync(ct);
            context.InsigniaRequiredRetos.RemoveRange(existing);

            var newRows = retoIds.Select(retoId => new InsigniaRequiredReto
            {
                InsigniaId = insigniaId,
                RetoId = retoId,
                CreatedAt = DateTime.UtcNow
            });
            await context.InsigniaRequiredRetos.AddRangeAsync(newRows, ct);
        }
    }
}
