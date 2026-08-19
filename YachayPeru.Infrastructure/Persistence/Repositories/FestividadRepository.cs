using Microsoft.EntityFrameworkCore;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Domain.Entities.Content;
using YachayPeru.Infrastructure.Persistence.SqlServer;
using System.Linq.Expressions;

namespace YachayPeru.Infrastructure.Persistence.Repositories
{
    public class FestividadRepository : IFestividadRepository
    {
        private readonly ApplicationDbContext context;

        public FestividadRepository(ApplicationDbContext _context) => context = _context;

        public async Task<Festividad> AddAsync(Festividad entity, CancellationToken ct = default)
        {
            await context.Festividades.AddAsync(entity, ct);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<Festividad> entities, CancellationToken ct = default)
            => await context.Festividades.AddRangeAsync(entities, ct);

        public async Task<bool> AnyAsync(Expression<Func<Festividad, bool>> predicate, CancellationToken ct = default)
            => await context.Festividades.Where(x => !x.Deleted).AnyAsync(predicate, ct);

        public async Task<int> CountAsync(Expression<Func<Festividad, bool>>? predicate = null, CancellationToken ct = default)
        {
            var q = context.Festividades.Where(x => !x.Deleted);
            if (predicate is not null) q = q.Where(predicate);
            return await q.CountAsync(ct);
        }

        public void Delete(Festividad entity) => context.Festividades.Remove(entity);
        public void DeleteRange(IEnumerable<Festividad> entities) => context.Festividades.RemoveRange(entities);

        public async Task<Festividad?> FirstOrDefaultAsync(Expression<Func<Festividad, bool>> predicate, CancellationToken ct = default)
            => await context.Festividades.Where(x => !x.Deleted).FirstOrDefaultAsync(predicate, ct);

        public async Task<Festividad?> GetByIdAsync(object id, CancellationToken ct = default)
        {
            if (id is not int fid) return null;
            return await context.Festividades.FirstOrDefaultAsync(x => x.Id == fid && !x.Deleted, ct);
        }

        public async Task<IReadOnlyList<Festividad>> ListAsync(CancellationToken ct = default)
            => await context.Festividades.Where(x => !x.Deleted).ToListAsync(ct);

        public async Task<IReadOnlyList<Festividad>> ListAsync(Expression<Func<Festividad, bool>> predicate, CancellationToken ct = default)
            => await context.Festividades.Where(x => !x.Deleted).Where(predicate).ToListAsync(ct);

        public async Task<Festividad?> SingleOrDefaultAsync(Expression<Func<Festividad, bool>> predicate, CancellationToken ct = default)
            => await context.Festividades.Where(x => !x.Deleted).SingleOrDefaultAsync(predicate, ct);

        public void Update(Festividad entity) => context.Festividades.Update(entity);
    }
}
