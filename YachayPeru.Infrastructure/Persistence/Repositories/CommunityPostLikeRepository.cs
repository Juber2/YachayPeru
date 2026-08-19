using Microsoft.EntityFrameworkCore;
using YachayPeru.Application.Abstractions.Persistence.Aprendiz;
using YachayPeru.Domain.Entities.Aprendiz;
using YachayPeru.Infrastructure.Persistence.SqlServer;
using System.Linq.Expressions;

namespace YachayPeru.Infrastructure.Persistence.Repositories
{
    public class CommunityPostLikeRepository : ICommunityPostLikeRepository
    {
        private readonly ApplicationDbContext context;

        public CommunityPostLikeRepository(ApplicationDbContext _context) => context = _context;

        public async Task<CommunityPostLike> AddAsync(CommunityPostLike entity, CancellationToken ct = default)
        {
            await context.CommunityPostLikes.AddAsync(entity, ct);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<CommunityPostLike> entities, CancellationToken ct = default)
            => await context.CommunityPostLikes.AddRangeAsync(entities, ct);

        public async Task<bool> AnyAsync(Expression<Func<CommunityPostLike, bool>> predicate, CancellationToken ct = default)
            => await context.CommunityPostLikes.Where(x => !x.Deleted).AnyAsync(predicate, ct);

        public async Task<int> CountAsync(Expression<Func<CommunityPostLike, bool>>? predicate = null, CancellationToken ct = default)
        {
            var q = context.CommunityPostLikes.Where(x => !x.Deleted);
            if (predicate is not null) q = q.Where(predicate);
            return await q.CountAsync(ct);
        }

        public void Delete(CommunityPostLike entity) => context.CommunityPostLikes.Remove(entity);
        public void DeleteRange(IEnumerable<CommunityPostLike> entities) => context.CommunityPostLikes.RemoveRange(entities);

        public async Task<CommunityPostLike?> FirstOrDefaultAsync(Expression<Func<CommunityPostLike, bool>> predicate, CancellationToken ct = default)
            => await context.CommunityPostLikes.Where(x => !x.Deleted).FirstOrDefaultAsync(predicate, ct);

        public async Task<CommunityPostLike?> GetByIdAsync(object id, CancellationToken ct = default)
        {
            if (id is not int lid) return null;
            return await context.CommunityPostLikes.FirstOrDefaultAsync(x => x.Id == lid && !x.Deleted, ct);
        }

        public async Task<IReadOnlyList<CommunityPostLike>> ListAsync(CancellationToken ct = default)
            => await context.CommunityPostLikes.Where(x => !x.Deleted).ToListAsync(ct);

        public async Task<IReadOnlyList<CommunityPostLike>> ListAsync(Expression<Func<CommunityPostLike, bool>> predicate, CancellationToken ct = default)
            => await context.CommunityPostLikes.Where(x => !x.Deleted).Where(predicate).ToListAsync(ct);

        public async Task<CommunityPostLike?> SingleOrDefaultAsync(Expression<Func<CommunityPostLike, bool>> predicate, CancellationToken ct = default)
            => await context.CommunityPostLikes.Where(x => !x.Deleted).SingleOrDefaultAsync(predicate, ct);

        public void Update(CommunityPostLike entity) => context.CommunityPostLikes.Update(entity);

        public async Task<CommunityPostLike?> GetByPostAndUserAsync(int postId, int userId, CancellationToken ct = default)
            => await context.CommunityPostLikes
                .FirstOrDefaultAsync(x => x.PostId == postId && x.UserId == userId && !x.Deleted, ct);

        public async Task<int> CountByPostAsync(int postId, CancellationToken ct = default)
            => await context.CommunityPostLikes.CountAsync(x => x.PostId == postId && !x.Deleted, ct);

        public async Task<IReadOnlyDictionary<int, int>> CountByPostsAsync(IReadOnlyCollection<int> postIds, CancellationToken ct = default)
        {
            var counts = await context.CommunityPostLikes
                .Where(x => postIds.Contains(x.PostId) && !x.Deleted)
                .GroupBy(x => x.PostId)
                .Select(g => new { PostId = g.Key, Count = g.Count() })
                .ToListAsync(ct);

            return counts.ToDictionary(x => x.PostId, x => x.Count);
        }

        public async Task<IReadOnlyCollection<int>> GetLikedPostIdsAsync(int userId, IReadOnlyCollection<int> postIds, CancellationToken ct = default)
            => await context.CommunityPostLikes
                .Where(x => x.UserId == userId && postIds.Contains(x.PostId) && !x.Deleted)
                .Select(x => x.PostId)
                .ToListAsync(ct);
    }
}
