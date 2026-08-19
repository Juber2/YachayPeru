using Microsoft.EntityFrameworkCore;
using YachayPeru.Application.Abstractions.Persistence.Learning;
using YachayPeru.Domain.Entities.Learning;
using YachayPeru.Infrastructure.Persistence.SqlServer;
using System.Linq.Expressions;

namespace YachayPeru.Infrastructure.Persistence.Repositories
{
    public class CourseModuleRepository : ICourseModuleRepository
    {
        private readonly ApplicationDbContext context;

        public CourseModuleRepository(ApplicationDbContext _context) => context = _context;

        public async Task<CourseModule> AddAsync(CourseModule entity, CancellationToken ct = default)
        {
            await context.CourseModules.AddAsync(entity, ct);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<CourseModule> entities, CancellationToken ct = default)
            => await context.CourseModules.AddRangeAsync(entities, ct);

        public async Task<bool> AnyAsync(Expression<Func<CourseModule, bool>> predicate, CancellationToken ct = default)
            => await context.CourseModules.Where(x => !x.Deleted).AnyAsync(predicate, ct);

        public async Task<int> CountAsync(Expression<Func<CourseModule, bool>>? predicate = null, CancellationToken ct = default)
        {
            var q = context.CourseModules.Where(x => !x.Deleted);
            if (predicate is not null) q = q.Where(predicate);
            return await q.CountAsync(ct);
        }

        public void Delete(CourseModule entity) => context.CourseModules.Remove(entity);
        public void DeleteRange(IEnumerable<CourseModule> entities) => context.CourseModules.RemoveRange(entities);

        public async Task<CourseModule?> FirstOrDefaultAsync(Expression<Func<CourseModule, bool>> predicate, CancellationToken ct = default)
            => await context.CourseModules.Where(x => !x.Deleted).FirstOrDefaultAsync(predicate, ct);

        public async Task<CourseModule?> GetByIdAsync(object id, CancellationToken ct = default)
        {
            if (id is not int moduleId) return null;
            return await context.CourseModules.FirstOrDefaultAsync(x => x.Id == moduleId && !x.Deleted, ct);
        }

        public async Task<IReadOnlyList<CourseModule>> ListAsync(CancellationToken ct = default)
            => await context.CourseModules.Where(x => !x.Deleted).ToListAsync(ct);

        public async Task<IReadOnlyList<CourseModule>> ListAsync(Expression<Func<CourseModule, bool>> predicate, CancellationToken ct = default)
            => await context.CourseModules.Where(x => !x.Deleted).Where(predicate).ToListAsync(ct);

        public async Task<CourseModule?> SingleOrDefaultAsync(Expression<Func<CourseModule, bool>> predicate, CancellationToken ct = default)
            => await context.CourseModules.Where(x => !x.Deleted).SingleOrDefaultAsync(predicate, ct);

        public void Update(CourseModule entity) => context.CourseModules.Update(entity);

        public async Task<IReadOnlyList<CourseModule>> GetByVersionAsync(int courseVersionId, CancellationToken ct = default)
            => await context.CourseModules
                .Where(x => x.CourseVersionId == courseVersionId && !x.Deleted)
                .OrderBy(x => x.OrderIndex)
                .ToListAsync(ct);

        public async Task<int> GetNextOrderIndexAsync(int courseVersionId, CancellationToken ct = default)
        {
            var max = await context.CourseModules
                .Where(x => x.CourseVersionId == courseVersionId && !x.Deleted)
                .MaxAsync(x => (int?)x.OrderIndex, ct);
            return (max ?? 0) + 1;
        }
    }
}
