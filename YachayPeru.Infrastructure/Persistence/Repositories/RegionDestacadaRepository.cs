using Microsoft.EntityFrameworkCore;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Domain.Entities.Content;
using YachayPeru.Infrastructure.Persistence.SqlServer;
using System.Linq.Expressions;

namespace YachayPeru.Infrastructure.Persistence.Repositories
{
    public class RegionDestacadaRepository : IRegionDestacadaRepository
    {
        private readonly ApplicationDbContext context;

        public RegionDestacadaRepository(ApplicationDbContext _context) => context = _context;

        public async Task<RegionDestacada> AddAsync(RegionDestacada entity, CancellationToken ct = default)
        {
            await context.RegionesDestacadas.AddAsync(entity, ct);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<RegionDestacada> entities, CancellationToken ct = default)
            => await context.RegionesDestacadas.AddRangeAsync(entities, ct);

        public async Task<bool> AnyAsync(Expression<Func<RegionDestacada, bool>> predicate, CancellationToken ct = default)
            => await context.RegionesDestacadas.Where(x => !x.Deleted).AnyAsync(predicate, ct);

        public async Task<int> CountAsync(Expression<Func<RegionDestacada, bool>>? predicate = null, CancellationToken ct = default)
        {
            var q = context.RegionesDestacadas.Where(x => !x.Deleted);
            if (predicate is not null) q = q.Where(predicate);
            return await q.CountAsync(ct);
        }

        public void Delete(RegionDestacada entity) => context.RegionesDestacadas.Remove(entity);
        public void DeleteRange(IEnumerable<RegionDestacada> entities) => context.RegionesDestacadas.RemoveRange(entities);

        public async Task<RegionDestacada?> FirstOrDefaultAsync(Expression<Func<RegionDestacada, bool>> predicate, CancellationToken ct = default)
            => await context.RegionesDestacadas.Where(x => !x.Deleted).FirstOrDefaultAsync(predicate, ct);

        public async Task<RegionDestacada?> GetByIdAsync(object id, CancellationToken ct = default)
        {
            if (id is not int rdId) return null;
            return await context.RegionesDestacadas.FirstOrDefaultAsync(x => x.Id == rdId && !x.Deleted, ct);
        }

        public async Task<IReadOnlyList<RegionDestacada>> ListAsync(CancellationToken ct = default)
            => await context.RegionesDestacadas.Where(x => !x.Deleted).ToListAsync(ct);

        public async Task<IReadOnlyList<RegionDestacada>> ListAsync(Expression<Func<RegionDestacada, bool>> predicate, CancellationToken ct = default)
            => await context.RegionesDestacadas.Where(x => !x.Deleted).Where(predicate).ToListAsync(ct);

        public async Task<RegionDestacada?> SingleOrDefaultAsync(Expression<Func<RegionDestacada, bool>> predicate, CancellationToken ct = default)
            => await context.RegionesDestacadas.Where(x => !x.Deleted).SingleOrDefaultAsync(predicate, ct);

        public void Update(RegionDestacada entity) => context.RegionesDestacadas.Update(entity);
    }
}
