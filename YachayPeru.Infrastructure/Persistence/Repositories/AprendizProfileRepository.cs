using Microsoft.EntityFrameworkCore;
using YachayPeru.Application.Abstractions.Persistence.Aprendiz;
using YachayPeru.Domain.Entities.Aprendiz;
using YachayPeru.Infrastructure.Persistence.SqlServer;
using System.Linq.Expressions;

namespace YachayPeru.Infrastructure.Persistence.Repositories
{
    public class AprendizProfileRepository : IAprendizProfileRepository
    {
        private readonly ApplicationDbContext context;

        public AprendizProfileRepository(ApplicationDbContext _context) => context = _context;

        public async Task<AprendizProfile> AddAsync(AprendizProfile entity, CancellationToken ct = default)
        {
            await context.AprendizProfiles.AddAsync(entity, ct);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<AprendizProfile> entities, CancellationToken ct = default)
            => await context.AprendizProfiles.AddRangeAsync(entities, ct);

        public async Task<bool> AnyAsync(Expression<Func<AprendizProfile, bool>> predicate, CancellationToken ct = default)
            => await context.AprendizProfiles.Where(x => !x.Deleted).AnyAsync(predicate, ct);

        public async Task<int> CountAsync(Expression<Func<AprendizProfile, bool>>? predicate = null, CancellationToken ct = default)
        {
            var q = context.AprendizProfiles.Where(x => !x.Deleted);
            if (predicate is not null) q = q.Where(predicate);
            return await q.CountAsync(ct);
        }

        public void Delete(AprendizProfile entity) => context.AprendizProfiles.Remove(entity);
        public void DeleteRange(IEnumerable<AprendizProfile> entities) => context.AprendizProfiles.RemoveRange(entities);

        public async Task<AprendizProfile?> FirstOrDefaultAsync(Expression<Func<AprendizProfile, bool>> predicate, CancellationToken ct = default)
            => await context.AprendizProfiles.Where(x => !x.Deleted).FirstOrDefaultAsync(predicate, ct);

        public async Task<AprendizProfile?> GetByIdAsync(object id, CancellationToken ct = default)
        {
            if (id is not int pid) return null;
            return await context.AprendizProfiles.FirstOrDefaultAsync(x => x.Id == pid && !x.Deleted, ct);
        }

        public async Task<IReadOnlyList<AprendizProfile>> ListAsync(CancellationToken ct = default)
            => await context.AprendizProfiles.Where(x => !x.Deleted).ToListAsync(ct);

        public async Task<IReadOnlyList<AprendizProfile>> ListAsync(Expression<Func<AprendizProfile, bool>> predicate, CancellationToken ct = default)
            => await context.AprendizProfiles.Where(x => !x.Deleted).Where(predicate).ToListAsync(ct);

        public async Task<AprendizProfile?> SingleOrDefaultAsync(Expression<Func<AprendizProfile, bool>> predicate, CancellationToken ct = default)
            => await context.AprendizProfiles.Where(x => !x.Deleted).SingleOrDefaultAsync(predicate, ct);

        public void Update(AprendizProfile entity) => context.AprendizProfiles.Update(entity);

        public async Task<AprendizProfile?> GetByUserIdAsync(int userId, CancellationToken ct = default)
            => await context.AprendizProfiles.FirstOrDefaultAsync(x => x.UserId == userId && !x.Deleted, ct);
    }
}
