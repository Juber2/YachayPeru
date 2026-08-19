using Microsoft.EntityFrameworkCore;
using YachayPeru.Application.Abstractions.Persistence.Learning;
using YachayPeru.Domain.Entities.Learning;
using YachayPeru.Infrastructure.Persistence.SqlServer;
using System.Linq.Expressions;

namespace YachayPeru.Infrastructure.Persistence.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly ApplicationDbContext context;

        public CourseRepository(ApplicationDbContext _context) => context = _context;

        public async Task<Course> AddAsync(Course entity, CancellationToken ct = default)
        {
            await context.Courses.AddAsync(entity, ct);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<Course> entities, CancellationToken ct = default)
            => await context.Courses.AddRangeAsync(entities, ct);

        public async Task<bool> AnyAsync(Expression<Func<Course, bool>> predicate, CancellationToken ct = default)
            => await context.Courses.Where(x => !x.Deleted).AnyAsync(predicate, ct);

        public async Task<int> CountAsync(Expression<Func<Course, bool>>? predicate = null, CancellationToken ct = default)
        {
            var q = context.Courses.Where(x => !x.Deleted);
            if (predicate is not null) q = q.Where(predicate);
            return await q.CountAsync(ct);
        }

        public void Delete(Course entity) => context.Courses.Remove(entity);
        public void DeleteRange(IEnumerable<Course> entities) => context.Courses.RemoveRange(entities);

        public async Task<Course?> FirstOrDefaultAsync(Expression<Func<Course, bool>> predicate, CancellationToken ct = default)
            => await context.Courses.Where(x => !x.Deleted).FirstOrDefaultAsync(predicate, ct);

        public async Task<Course?> GetByIdAsync(object id, CancellationToken ct = default)
        {
            if (id is not int courseId) return null;
            return await context.Courses.FirstOrDefaultAsync(x => x.Id == courseId && !x.Deleted, ct);
        }

        public async Task<IReadOnlyList<Course>> ListAsync(CancellationToken ct = default)
            => await context.Courses.Where(x => !x.Deleted).ToListAsync(ct);

        public async Task<IReadOnlyList<Course>> ListAsync(Expression<Func<Course, bool>> predicate, CancellationToken ct = default)
            => await context.Courses.Where(x => !x.Deleted).Where(predicate).ToListAsync(ct);

        public async Task<Course?> SingleOrDefaultAsync(Expression<Func<Course, bool>> predicate, CancellationToken ct = default)
            => await context.Courses.Where(x => !x.Deleted).SingleOrDefaultAsync(predicate, ct);

        public void Update(Course entity) => context.Courses.Update(entity);
    }
}
