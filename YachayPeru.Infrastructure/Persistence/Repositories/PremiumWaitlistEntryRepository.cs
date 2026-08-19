using Microsoft.EntityFrameworkCore;
using YachayPeru.Application.Abstractions.Persistence.Aprendiz;
using YachayPeru.Domain.Entities.Aprendiz;
using YachayPeru.Infrastructure.Persistence.SqlServer;
using System.Linq.Expressions;

namespace YachayPeru.Infrastructure.Persistence.Repositories
{
    public class PremiumWaitlistEntryRepository : IPremiumWaitlistEntryRepository
    {
        private readonly ApplicationDbContext context;

        public PremiumWaitlistEntryRepository(ApplicationDbContext _context) => context = _context;

        public async Task<PremiumWaitlistEntry> AddAsync(PremiumWaitlistEntry entity, CancellationToken ct = default)
        {
            await context.PremiumWaitlistEntries.AddAsync(entity, ct);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<PremiumWaitlistEntry> entities, CancellationToken ct = default)
            => await context.PremiumWaitlistEntries.AddRangeAsync(entities, ct);

        public async Task<bool> AnyAsync(Expression<Func<PremiumWaitlistEntry, bool>> predicate, CancellationToken ct = default)
            => await context.PremiumWaitlistEntries.Where(x => !x.Deleted).AnyAsync(predicate, ct);

        public async Task<int> CountAsync(Expression<Func<PremiumWaitlistEntry, bool>>? predicate = null, CancellationToken ct = default)
        {
            var q = context.PremiumWaitlistEntries.Where(x => !x.Deleted);
            if (predicate is not null) q = q.Where(predicate);
            return await q.CountAsync(ct);
        }

        public void Delete(PremiumWaitlistEntry entity) => context.PremiumWaitlistEntries.Remove(entity);
        public void DeleteRange(IEnumerable<PremiumWaitlistEntry> entities) => context.PremiumWaitlistEntries.RemoveRange(entities);

        public async Task<PremiumWaitlistEntry?> FirstOrDefaultAsync(Expression<Func<PremiumWaitlistEntry, bool>> predicate, CancellationToken ct = default)
            => await context.PremiumWaitlistEntries.Where(x => !x.Deleted).FirstOrDefaultAsync(predicate, ct);

        public async Task<PremiumWaitlistEntry?> GetByIdAsync(object id, CancellationToken ct = default)
        {
            if (id is not int eid) return null;
            return await context.PremiumWaitlistEntries.FirstOrDefaultAsync(x => x.Id == eid && !x.Deleted, ct);
        }

        public async Task<IReadOnlyList<PremiumWaitlistEntry>> ListAsync(CancellationToken ct = default)
            => await context.PremiumWaitlistEntries.Where(x => !x.Deleted).ToListAsync(ct);

        public async Task<IReadOnlyList<PremiumWaitlistEntry>> ListAsync(Expression<Func<PremiumWaitlistEntry, bool>> predicate, CancellationToken ct = default)
            => await context.PremiumWaitlistEntries.Where(x => !x.Deleted).Where(predicate).ToListAsync(ct);

        public async Task<PremiumWaitlistEntry?> SingleOrDefaultAsync(Expression<Func<PremiumWaitlistEntry, bool>> predicate, CancellationToken ct = default)
            => await context.PremiumWaitlistEntries.Where(x => !x.Deleted).SingleOrDefaultAsync(predicate, ct);

        public void Update(PremiumWaitlistEntry entity) => context.PremiumWaitlistEntries.Update(entity);

        public async Task<PremiumWaitlistEntry?> GetByUserIdAsync(int userId, CancellationToken ct = default)
            => await context.PremiumWaitlistEntries.FirstOrDefaultAsync(x => x.UserId == userId && !x.Deleted, ct);
    }
}
