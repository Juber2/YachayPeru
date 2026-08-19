using Microsoft.EntityFrameworkCore;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Domain.Entities.Content;
using YachayPeru.Infrastructure.Persistence.SqlServer;
using System.Linq.Expressions;

namespace YachayPeru.Infrastructure.Persistence.Repositories
{
    public class PredisenoRepository : IPredisenoRepository
    {
        private readonly ApplicationDbContext context;

        public PredisenoRepository(ApplicationDbContext _context) => context = _context;

        public async Task<Prediseno> AddAsync(Prediseno entity, CancellationToken ct = default)
        {
            await context.Predisenos.AddAsync(entity, ct);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<Prediseno> entities, CancellationToken ct = default)
            => await context.Predisenos.AddRangeAsync(entities, ct);

        public async Task<bool> AnyAsync(Expression<Func<Prediseno, bool>> predicate, CancellationToken ct = default)
            => await context.Predisenos.Where(x => !x.Deleted).AnyAsync(predicate, ct);

        public async Task<int> CountAsync(Expression<Func<Prediseno, bool>>? predicate = null, CancellationToken ct = default)
        {
            var q = context.Predisenos.Where(x => !x.Deleted);
            if (predicate is not null) q = q.Where(predicate);
            return await q.CountAsync(ct);
        }

        public void Delete(Prediseno entity) => context.Predisenos.Remove(entity);
        public void DeleteRange(IEnumerable<Prediseno> entities) => context.Predisenos.RemoveRange(entities);

        public async Task<Prediseno?> FirstOrDefaultAsync(Expression<Func<Prediseno, bool>> predicate, CancellationToken ct = default)
            => await context.Predisenos.Where(x => !x.Deleted).FirstOrDefaultAsync(predicate, ct);

        public async Task<Prediseno?> GetByIdAsync(object id, CancellationToken ct = default)
        {
            if (id is not int predisenoId) return null;
            return await context.Predisenos.FirstOrDefaultAsync(x => x.Id == predisenoId && !x.Deleted, ct);
        }

        public async Task<IReadOnlyList<Prediseno>> ListAsync(CancellationToken ct = default)
            => await context.Predisenos.Where(x => !x.Deleted).ToListAsync(ct);

        public async Task<IReadOnlyList<Prediseno>> ListAsync(Expression<Func<Prediseno, bool>> predicate, CancellationToken ct = default)
            => await context.Predisenos.Where(x => !x.Deleted).Where(predicate).ToListAsync(ct);

        public async Task<Prediseno?> SingleOrDefaultAsync(Expression<Func<Prediseno, bool>> predicate, CancellationToken ct = default)
            => await context.Predisenos.Where(x => !x.Deleted).SingleOrDefaultAsync(predicate, ct);

        public void Update(Prediseno entity) => context.Predisenos.Update(entity);
    }
}
