using Microsoft.EntityFrameworkCore;
using YachayPeru.Application.Abstractions.Persistence.Aprendiz;
using YachayPeru.Domain.Entities.Aprendiz;
using YachayPeru.Infrastructure.Persistence.SqlServer;
using System.Linq.Expressions;

namespace YachayPeru.Infrastructure.Persistence.Repositories
{
    public class CommunityPostRepository : ICommunityPostRepository
    {
        private readonly ApplicationDbContext context;

        public CommunityPostRepository(ApplicationDbContext _context) => context = _context;

        public async Task<CommunityPost> AddAsync(CommunityPost entity, CancellationToken ct = default)
        {
            await context.CommunityPosts.AddAsync(entity, ct);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<CommunityPost> entities, CancellationToken ct = default)
            => await context.CommunityPosts.AddRangeAsync(entities, ct);

        public async Task<bool> AnyAsync(Expression<Func<CommunityPost, bool>> predicate, CancellationToken ct = default)
            => await context.CommunityPosts.Where(x => !x.Deleted).AnyAsync(predicate, ct);

        public async Task<int> CountAsync(Expression<Func<CommunityPost, bool>>? predicate = null, CancellationToken ct = default)
        {
            var q = context.CommunityPosts.Where(x => !x.Deleted);
            if (predicate is not null) q = q.Where(predicate);
            return await q.CountAsync(ct);
        }

        public void Delete(CommunityPost entity) => context.CommunityPosts.Remove(entity);
        public void DeleteRange(IEnumerable<CommunityPost> entities) => context.CommunityPosts.RemoveRange(entities);

        public async Task<CommunityPost?> FirstOrDefaultAsync(Expression<Func<CommunityPost, bool>> predicate, CancellationToken ct = default)
            => await context.CommunityPosts.Where(x => !x.Deleted).FirstOrDefaultAsync(predicate, ct);

        public async Task<CommunityPost?> GetByIdAsync(object id, CancellationToken ct = default)
        {
            if (id is not int pid) return null;
            return await context.CommunityPosts.FirstOrDefaultAsync(x => x.Id == pid && !x.Deleted, ct);
        }

        public async Task<IReadOnlyList<CommunityPost>> ListAsync(CancellationToken ct = default)
            => await context.CommunityPosts.Where(x => !x.Deleted).OrderByDescending(x => x.CreatedAt).ToListAsync(ct);

        public async Task<IReadOnlyList<CommunityPost>> ListAsync(Expression<Func<CommunityPost, bool>> predicate, CancellationToken ct = default)
            => await context.CommunityPosts.Where(x => !x.Deleted).Where(predicate).ToListAsync(ct);

        public async Task<CommunityPost?> SingleOrDefaultAsync(Expression<Func<CommunityPost, bool>> predicate, CancellationToken ct = default)
            => await context.CommunityPosts.Where(x => !x.Deleted).SingleOrDefaultAsync(predicate, ct);

        public void Update(CommunityPost entity) => context.CommunityPosts.Update(entity);
    }
}
