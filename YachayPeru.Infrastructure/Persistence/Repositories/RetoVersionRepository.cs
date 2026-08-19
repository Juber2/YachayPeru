using Microsoft.EntityFrameworkCore;
using YachayPeru.Application.Abstractions.Persistence.Assessment;
using YachayPeru.Domain.Constants;
using YachayPeru.Domain.Entities.Assessment;
using YachayPeru.Infrastructure.Persistence.SqlServer;
using System.Linq.Expressions;

namespace YachayPeru.Infrastructure.Persistence.Repositories
{
    public class RetoVersionRepository : IRetoVersionRepository
    {
        private readonly ApplicationDbContext context;

        public RetoVersionRepository(ApplicationDbContext _context) => context = _context;

        public async Task<RetoVersion> AddAsync(RetoVersion entity, CancellationToken ct = default)
        {
            await context.RetoVersions.AddAsync(entity, ct);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<RetoVersion> entities, CancellationToken ct = default)
            => await context.RetoVersions.AddRangeAsync(entities, ct);

        public async Task<bool> AnyAsync(Expression<Func<RetoVersion, bool>> predicate, CancellationToken ct = default)
            => await context.RetoVersions.Where(x => !x.Deleted).AnyAsync(predicate, ct);

        public async Task<int> CountAsync(Expression<Func<RetoVersion, bool>>? predicate = null, CancellationToken ct = default)
        {
            var q = context.RetoVersions.Where(x => !x.Deleted);
            if (predicate is not null) q = q.Where(predicate);
            return await q.CountAsync(ct);
        }

        public void Delete(RetoVersion entity) => context.RetoVersions.Remove(entity);
        public void DeleteRange(IEnumerable<RetoVersion> entities) => context.RetoVersions.RemoveRange(entities);

        public async Task<RetoVersion?> FirstOrDefaultAsync(Expression<Func<RetoVersion, bool>> predicate, CancellationToken ct = default)
            => await context.RetoVersions.Where(x => !x.Deleted).FirstOrDefaultAsync(predicate, ct);

        public async Task<RetoVersion?> GetByIdAsync(object id, CancellationToken ct = default)
        {
            if (id is not int versionId) return null;
            return await context.RetoVersions.FirstOrDefaultAsync(x => x.Id == versionId && !x.Deleted, ct);
        }

        public async Task<IReadOnlyList<RetoVersion>> ListAsync(CancellationToken ct = default)
            => await context.RetoVersions.Where(x => !x.Deleted).ToListAsync(ct);

        public async Task<IReadOnlyList<RetoVersion>> ListAsync(Expression<Func<RetoVersion, bool>> predicate, CancellationToken ct = default)
            => await context.RetoVersions.Where(x => !x.Deleted).Where(predicate).ToListAsync(ct);

        public async Task<RetoVersion?> SingleOrDefaultAsync(Expression<Func<RetoVersion, bool>> predicate, CancellationToken ct = default)
            => await context.RetoVersions.Where(x => !x.Deleted).SingleOrDefaultAsync(predicate, ct);

        public void Update(RetoVersion entity) => context.RetoVersions.Update(entity);

        public async Task<RetoVersion?> GetPublishedByRetoAsync(int retoId, CancellationToken ct = default)
            => await context.RetoVersions
                .Where(x => x.RetoId == retoId
                    && x.StatusCode == AppConstants.RetoVersionStatus.Published && !x.Deleted)
                .OrderByDescending(x => x.VersionNumber)
                .FirstOrDefaultAsync(ct);

        public async Task<RetoVersion?> GetDraftByRetoAsync(int retoId, CancellationToken ct = default)
            => await context.RetoVersions
                .FirstOrDefaultAsync(x => x.RetoId == retoId
                    && x.StatusCode == AppConstants.RetoVersionStatus.Draft && !x.Deleted, ct);

        public async Task<IReadOnlyList<RetoVersion>> GetHistoryByRetoAsync(int retoId, CancellationToken ct = default)
            => await context.RetoVersions
                .Where(x => x.RetoId == retoId && !x.Deleted)
                .OrderByDescending(x => x.VersionNumber)
                .ToListAsync(ct);

        public async Task<int> GetNextVersionNumberAsync(int retoId, CancellationToken ct = default)
        {
            var max = await context.RetoVersions
                .Where(x => x.RetoId == retoId && !x.Deleted)
                .MaxAsync(x => (int?)x.VersionNumber, ct);
            return (max ?? 0) + 1;
        }
    }
}
