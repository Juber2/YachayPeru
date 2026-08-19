using Microsoft.EntityFrameworkCore;
using YachayPeru.Application.Abstractions.Persistence.Learning;
using YachayPeru.Domain.Constants;
using YachayPeru.Domain.Entities.Learning;
using YachayPeru.Infrastructure.Persistence.SqlServer;
using System.Linq.Expressions;

namespace YachayPeru.Infrastructure.Persistence.Repositories
{
    public class CourseVersionRepository : ICourseVersionRepository
    {
        private readonly ApplicationDbContext context;

        public CourseVersionRepository(ApplicationDbContext _context) => context = _context;

        public async Task<CourseVersion> AddAsync(CourseVersion entity, CancellationToken ct = default)
        {
            await context.CourseVersions.AddAsync(entity, ct);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<CourseVersion> entities, CancellationToken ct = default)
            => await context.CourseVersions.AddRangeAsync(entities, ct);

        public async Task<bool> AnyAsync(Expression<Func<CourseVersion, bool>> predicate, CancellationToken ct = default)
            => await context.CourseVersions.Where(x => !x.Deleted).AnyAsync(predicate, ct);

        public async Task<int> CountAsync(Expression<Func<CourseVersion, bool>>? predicate = null, CancellationToken ct = default)
        {
            var q = context.CourseVersions.Where(x => !x.Deleted);
            if (predicate is not null) q = q.Where(predicate);
            return await q.CountAsync(ct);
        }

        public void Delete(CourseVersion entity) => context.CourseVersions.Remove(entity);
        public void DeleteRange(IEnumerable<CourseVersion> entities) => context.CourseVersions.RemoveRange(entities);

        public async Task<CourseVersion?> FirstOrDefaultAsync(Expression<Func<CourseVersion, bool>> predicate, CancellationToken ct = default)
            => await context.CourseVersions.Where(x => !x.Deleted).FirstOrDefaultAsync(predicate, ct);

        public async Task<CourseVersion?> GetByIdAsync(object id, CancellationToken ct = default)
        {
            if (id is not int versionId) return null;
            return await context.CourseVersions.FirstOrDefaultAsync(x => x.Id == versionId && !x.Deleted, ct);
        }

        public async Task<IReadOnlyList<CourseVersion>> ListAsync(CancellationToken ct = default)
            => await context.CourseVersions.Where(x => !x.Deleted).ToListAsync(ct);

        public async Task<IReadOnlyList<CourseVersion>> ListAsync(Expression<Func<CourseVersion, bool>> predicate, CancellationToken ct = default)
            => await context.CourseVersions.Where(x => !x.Deleted).Where(predicate).ToListAsync(ct);

        public async Task<CourseVersion?> SingleOrDefaultAsync(Expression<Func<CourseVersion, bool>> predicate, CancellationToken ct = default)
            => await context.CourseVersions.Where(x => !x.Deleted).SingleOrDefaultAsync(predicate, ct);

        public void Update(CourseVersion entity) => context.CourseVersions.Update(entity);

        public async Task<CourseVersion?> GetCurrentAsync(int courseId, CancellationToken ct = default)
            => await context.CourseVersions
                .FirstOrDefaultAsync(x => x.CourseId == courseId && x.IsCurrent && !x.Deleted, ct);

        public async Task<CourseVersion?> GetDraftAsync(int courseId, CancellationToken ct = default)
            => await context.CourseVersions
                .FirstOrDefaultAsync(x => x.CourseId == courseId
                    && x.StatusCode == AppConstants.CourseVersionStatus.Draft && !x.Deleted, ct);

        public async Task<IReadOnlyList<CourseVersion>> GetHistoryAsync(int courseId, CancellationToken ct = default)
            => await context.CourseVersions
                .Where(x => x.CourseId == courseId && !x.Deleted)
                .OrderByDescending(x => x.VersionNumber)
                .ToListAsync(ct);

        public async Task<int> GetNextVersionNumberAsync(int courseId, CancellationToken ct = default)
        {
            var max = await context.CourseVersions
                .Where(x => x.CourseId == courseId && !x.Deleted)
                .MaxAsync(x => (int?)x.VersionNumber, ct);
            return (max ?? 0) + 1;
        }
    }
}
