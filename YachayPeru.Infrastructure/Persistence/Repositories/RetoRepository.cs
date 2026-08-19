using Microsoft.EntityFrameworkCore;
using YachayPeru.Application.Abstractions.Persistence.Assessment;
using YachayPeru.Domain.Entities.Assessment;
using YachayPeru.Infrastructure.Persistence.SqlServer;
using System.Linq.Expressions;

namespace YachayPeru.Infrastructure.Persistence.Repositories
{
    public class RetoRepository : IRetoRepository
    {
        private readonly ApplicationDbContext context;

        public RetoRepository(ApplicationDbContext _context) => context = _context;

        public async Task<Reto> AddAsync(Reto entity, CancellationToken ct = default)
        {
            await context.Retos.AddAsync(entity, ct);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<Reto> entities, CancellationToken ct = default)
            => await context.Retos.AddRangeAsync(entities, ct);

        public async Task<bool> AnyAsync(Expression<Func<Reto, bool>> predicate, CancellationToken ct = default)
            => await context.Retos.Where(x => !x.Deleted).AnyAsync(predicate, ct);

        public async Task<int> CountAsync(Expression<Func<Reto, bool>>? predicate = null, CancellationToken ct = default)
        {
            var q = context.Retos.Where(x => !x.Deleted);
            if (predicate is not null) q = q.Where(predicate);
            return await q.CountAsync(ct);
        }

        public void Delete(Reto entity) => context.Retos.Remove(entity);
        public void DeleteRange(IEnumerable<Reto> entities) => context.Retos.RemoveRange(entities);

        public async Task<Reto?> FirstOrDefaultAsync(Expression<Func<Reto, bool>> predicate, CancellationToken ct = default)
            => await context.Retos.Where(x => !x.Deleted).FirstOrDefaultAsync(predicate, ct);

        public async Task<Reto?> GetByIdAsync(object id, CancellationToken ct = default)
        {
            if (id is not int retoId) return null;
            return await context.Retos.FirstOrDefaultAsync(x => x.Id == retoId && !x.Deleted, ct);
        }

        public async Task<IReadOnlyList<Reto>> ListAsync(CancellationToken ct = default)
            => await context.Retos.Where(x => !x.Deleted).ToListAsync(ct);

        public async Task<IReadOnlyList<Reto>> ListAsync(Expression<Func<Reto, bool>> predicate, CancellationToken ct = default)
            => await context.Retos.Where(x => !x.Deleted).Where(predicate).ToListAsync(ct);

        public async Task<Reto?> SingleOrDefaultAsync(Expression<Func<Reto, bool>> predicate, CancellationToken ct = default)
            => await context.Retos.Where(x => !x.Deleted).SingleOrDefaultAsync(predicate, ct);

        public void Update(Reto entity) => context.Retos.Update(entity);

        public async Task<IReadOnlyList<Reto>> GetByCourseAsync(int courseId, CancellationToken ct = default)
            => await context.Retos
                .Where(x => !x.Deleted && x.CourseId == courseId)
                .OrderBy(x => x.CreatedAt)
                .ToListAsync(ct);

        public async Task<IReadOnlyList<RetoWithCourseRow>> GetAllWithCourseAsync(CancellationToken ct = default)
            => await (
                from reto in context.Retos
                where !reto.Deleted
                join course in context.Courses on reto.CourseId equals course.Id
                where !course.Deleted
                orderby reto.CreatedAt
                select new RetoWithCourseRow(reto.Id, course.Id, course.Title, reto.CreatedAt)
            ).ToListAsync(ct);
    }
}
