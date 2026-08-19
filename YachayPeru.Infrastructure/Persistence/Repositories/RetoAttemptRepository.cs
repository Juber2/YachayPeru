using Microsoft.EntityFrameworkCore;
using YachayPeru.Application.Abstractions.Persistence.Aprendiz;
using YachayPeru.Domain.Entities.Aprendiz;
using YachayPeru.Infrastructure.Persistence.SqlServer;
using System.Linq.Expressions;

namespace YachayPeru.Infrastructure.Persistence.Repositories
{
    public class RetoAttemptRepository : IRetoAttemptRepository
    {
        private readonly ApplicationDbContext context;

        public RetoAttemptRepository(ApplicationDbContext _context) => context = _context;

        public async Task<RetoAttempt> AddAsync(RetoAttempt entity, CancellationToken ct = default)
        {
            await context.RetoAttempts.AddAsync(entity, ct);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<RetoAttempt> entities, CancellationToken ct = default)
            => await context.RetoAttempts.AddRangeAsync(entities, ct);

        public async Task<bool> AnyAsync(Expression<Func<RetoAttempt, bool>> predicate, CancellationToken ct = default)
            => await context.RetoAttempts.Where(x => !x.Deleted).AnyAsync(predicate, ct);

        public async Task<int> CountAsync(Expression<Func<RetoAttempt, bool>>? predicate = null, CancellationToken ct = default)
        {
            var q = context.RetoAttempts.Where(x => !x.Deleted);
            if (predicate is not null) q = q.Where(predicate);
            return await q.CountAsync(ct);
        }

        public void Delete(RetoAttempt entity) => context.RetoAttempts.Remove(entity);
        public void DeleteRange(IEnumerable<RetoAttempt> entities) => context.RetoAttempts.RemoveRange(entities);

        public async Task<RetoAttempt?> FirstOrDefaultAsync(Expression<Func<RetoAttempt, bool>> predicate, CancellationToken ct = default)
            => await context.RetoAttempts.Where(x => !x.Deleted).FirstOrDefaultAsync(predicate, ct);

        public async Task<RetoAttempt?> GetByIdAsync(object id, CancellationToken ct = default)
        {
            if (id is not int aid) return null;
            return await context.RetoAttempts.FirstOrDefaultAsync(x => x.Id == aid && !x.Deleted, ct);
        }

        public async Task<IReadOnlyList<RetoAttempt>> ListAsync(CancellationToken ct = default)
            => await context.RetoAttempts.Where(x => !x.Deleted).ToListAsync(ct);

        public async Task<IReadOnlyList<RetoAttempt>> ListAsync(Expression<Func<RetoAttempt, bool>> predicate, CancellationToken ct = default)
            => await context.RetoAttempts.Where(x => !x.Deleted).Where(predicate).ToListAsync(ct);

        public async Task<RetoAttempt?> SingleOrDefaultAsync(Expression<Func<RetoAttempt, bool>> predicate, CancellationToken ct = default)
            => await context.RetoAttempts.Where(x => !x.Deleted).SingleOrDefaultAsync(predicate, ct);

        public void Update(RetoAttempt entity) => context.RetoAttempts.Update(entity);

        public async Task<int> CountByUserAndRetoAsync(int userId, int retoId, CancellationToken ct = default)
            => await context.RetoAttempts.CountAsync(x => x.UserId == userId && x.RetoId == retoId && !x.Deleted, ct);

        public async Task<bool> HasPassedAsync(int userId, int retoId, CancellationToken ct = default)
            => await context.RetoAttempts.AnyAsync(x => x.UserId == userId && x.RetoId == retoId && x.Passed && !x.Deleted, ct);

        public async Task<IReadOnlyList<int>> GetPassedRetoIdsByUserAsync(int userId, CancellationToken ct = default)
            => await context.RetoAttempts
                .Where(x => x.UserId == userId && x.Passed && !x.Deleted)
                .Select(x => x.RetoId)
                .Distinct()
                .ToListAsync(ct);

        public async Task<RetoAttempt?> GetBestByUserAndRetoAsync(int userId, int retoId, CancellationToken ct = default)
            => await context.RetoAttempts
                .Where(x => x.UserId == userId && x.RetoId == retoId && !x.Deleted)
                .OrderByDescending(x => x.EarnedPoints)
                .FirstOrDefaultAsync(ct);

        public async Task<int> CountDistinctPerfectRetosByUserAsync(int userId, CancellationToken ct = default)
            => await context.RetoAttempts
                .Where(x => x.UserId == userId && x.Passed && x.TotalQuestions > 0 && x.CorrectCount == x.TotalQuestions && !x.Deleted)
                .Select(x => x.RetoId)
                .Distinct()
                .CountAsync(ct);
    }
}
