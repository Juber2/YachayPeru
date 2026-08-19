using Microsoft.EntityFrameworkCore;
using YachayPeru.Application.Abstractions.Persistence.Aprendiz;
using YachayPeru.Domain.Entities.Aprendiz;
using YachayPeru.Infrastructure.Persistence.SqlServer;
using System.Linq.Expressions;

namespace YachayPeru.Infrastructure.Persistence.Repositories
{
    public class AprendizInsigniaEarnedRepository : IAprendizInsigniaEarnedRepository
    {
        private readonly ApplicationDbContext context;

        public AprendizInsigniaEarnedRepository(ApplicationDbContext _context) => context = _context;

        public async Task<AprendizInsigniaEarned> AddAsync(AprendizInsigniaEarned entity, CancellationToken ct = default)
        {
            await context.AprendizInsigniasEarned.AddAsync(entity, ct);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<AprendizInsigniaEarned> entities, CancellationToken ct = default)
            => await context.AprendizInsigniasEarned.AddRangeAsync(entities, ct);

        public async Task<bool> AnyAsync(Expression<Func<AprendizInsigniaEarned, bool>> predicate, CancellationToken ct = default)
            => await context.AprendizInsigniasEarned.Where(x => !x.Deleted).AnyAsync(predicate, ct);

        public async Task<int> CountAsync(Expression<Func<AprendizInsigniaEarned, bool>>? predicate = null, CancellationToken ct = default)
        {
            var q = context.AprendizInsigniasEarned.Where(x => !x.Deleted);
            if (predicate is not null) q = q.Where(predicate);
            return await q.CountAsync(ct);
        }

        public void Delete(AprendizInsigniaEarned entity) => context.AprendizInsigniasEarned.Remove(entity);
        public void DeleteRange(IEnumerable<AprendizInsigniaEarned> entities) => context.AprendizInsigniasEarned.RemoveRange(entities);

        public async Task<AprendizInsigniaEarned?> FirstOrDefaultAsync(Expression<Func<AprendizInsigniaEarned, bool>> predicate, CancellationToken ct = default)
            => await context.AprendizInsigniasEarned.Where(x => !x.Deleted).FirstOrDefaultAsync(predicate, ct);

        public async Task<AprendizInsigniaEarned?> GetByIdAsync(object id, CancellationToken ct = default)
        {
            if (id is not int eid) return null;
            return await context.AprendizInsigniasEarned.FirstOrDefaultAsync(x => x.Id == eid && !x.Deleted, ct);
        }

        public async Task<IReadOnlyList<AprendizInsigniaEarned>> ListAsync(CancellationToken ct = default)
            => await context.AprendizInsigniasEarned.Where(x => !x.Deleted).ToListAsync(ct);

        public async Task<IReadOnlyList<AprendizInsigniaEarned>> ListAsync(Expression<Func<AprendizInsigniaEarned, bool>> predicate, CancellationToken ct = default)
            => await context.AprendizInsigniasEarned.Where(x => !x.Deleted).Where(predicate).ToListAsync(ct);

        public async Task<AprendizInsigniaEarned?> SingleOrDefaultAsync(Expression<Func<AprendizInsigniaEarned, bool>> predicate, CancellationToken ct = default)
            => await context.AprendizInsigniasEarned.Where(x => !x.Deleted).SingleOrDefaultAsync(predicate, ct);

        public void Update(AprendizInsigniaEarned entity) => context.AprendizInsigniasEarned.Update(entity);

        public async Task<IReadOnlyList<AprendizInsigniaEarned>> GetByUserAsync(int userId, CancellationToken ct = default)
            => await context.AprendizInsigniasEarned.Where(x => x.UserId == userId && !x.Deleted).ToListAsync(ct);

        public async Task<bool> HasEarnedAsync(int userId, int insigniaId, CancellationToken ct = default)
            => await context.AprendizInsigniasEarned.AnyAsync(x => x.UserId == userId && x.InsigniaId == insigniaId && !x.Deleted, ct);
    }
}
