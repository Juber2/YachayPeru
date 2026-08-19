using Microsoft.EntityFrameworkCore;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Domain.Entities.Content;
using YachayPeru.Infrastructure.Persistence.SqlServer;
using System.Linq.Expressions;

namespace YachayPeru.Infrastructure.Persistence.Repositories
{
    public class MediaItemRepository : IMediaItemRepository
    {
        private readonly ApplicationDbContext context;

        public MediaItemRepository(ApplicationDbContext _context) => context = _context;

        public async Task<MediaItem> AddAsync(MediaItem entity, CancellationToken ct = default)
        {
            await context.MediaItems.AddAsync(entity, ct);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<MediaItem> entities, CancellationToken ct = default)
            => await context.MediaItems.AddRangeAsync(entities, ct);

        public async Task<bool> AnyAsync(Expression<Func<MediaItem, bool>> predicate, CancellationToken ct = default)
            => await context.MediaItems.Where(x => !x.Deleted).AnyAsync(predicate, ct);

        public async Task<int> CountAsync(Expression<Func<MediaItem, bool>>? predicate = null, CancellationToken ct = default)
        {
            var q = context.MediaItems.Where(x => !x.Deleted);
            if (predicate is not null) q = q.Where(predicate);
            return await q.CountAsync(ct);
        }

        public void Delete(MediaItem entity) => context.MediaItems.Remove(entity);
        public void DeleteRange(IEnumerable<MediaItem> entities) => context.MediaItems.RemoveRange(entities);

        public async Task<MediaItem?> FirstOrDefaultAsync(Expression<Func<MediaItem, bool>> predicate, CancellationToken ct = default)
            => await context.MediaItems.Where(x => !x.Deleted).FirstOrDefaultAsync(predicate, ct);

        public async Task<MediaItem?> GetByIdAsync(object id, CancellationToken ct = default)
        {
            if (id is not int mid) return null;
            return await context.MediaItems.FirstOrDefaultAsync(x => x.Id == mid && !x.Deleted, ct);
        }

        public async Task<IReadOnlyList<MediaItem>> ListAsync(CancellationToken ct = default)
            => await context.MediaItems.Where(x => !x.Deleted).ToListAsync(ct);

        public async Task<IReadOnlyList<MediaItem>> ListAsync(Expression<Func<MediaItem, bool>> predicate, CancellationToken ct = default)
            => await context.MediaItems.Where(x => !x.Deleted).Where(predicate).ToListAsync(ct);

        public async Task<MediaItem?> SingleOrDefaultAsync(Expression<Func<MediaItem, bool>> predicate, CancellationToken ct = default)
            => await context.MediaItems.Where(x => !x.Deleted).SingleOrDefaultAsync(predicate, ct);

        public void Update(MediaItem entity) => context.MediaItems.Update(entity);
    }
}
