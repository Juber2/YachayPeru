using Microsoft.EntityFrameworkCore;
using YachayPeru.Application.Abstractions.Persistence.Learning;
using YachayPeru.Domain.Entities.Learning;
using YachayPeru.Infrastructure.Persistence.SqlServer;
using System.Linq.Expressions;

namespace YachayPeru.Infrastructure.Persistence.Repositories
{
    public class ModuleContentRepository : IModuleContentRepository
    {
        private readonly ApplicationDbContext context;

        public ModuleContentRepository(ApplicationDbContext _context) => context = _context;

        public async Task<ModuleContent> AddAsync(ModuleContent entity, CancellationToken ct = default)
        {
            await context.ModuleContents.AddAsync(entity, ct);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<ModuleContent> entities, CancellationToken ct = default)
            => await context.ModuleContents.AddRangeAsync(entities, ct);

        public async Task<bool> AnyAsync(Expression<Func<ModuleContent, bool>> predicate, CancellationToken ct = default)
            => await context.ModuleContents.Where(x => !x.Deleted).AnyAsync(predicate, ct);

        public async Task<int> CountAsync(Expression<Func<ModuleContent, bool>>? predicate = null, CancellationToken ct = default)
        {
            var q = context.ModuleContents.Where(x => !x.Deleted);
            if (predicate is not null) q = q.Where(predicate);
            return await q.CountAsync(ct);
        }

        public void Delete(ModuleContent entity) => context.ModuleContents.Remove(entity);
        public void DeleteRange(IEnumerable<ModuleContent> entities) => context.ModuleContents.RemoveRange(entities);

        public async Task<ModuleContent?> FirstOrDefaultAsync(Expression<Func<ModuleContent, bool>> predicate, CancellationToken ct = default)
            => await context.ModuleContents.Where(x => !x.Deleted).FirstOrDefaultAsync(predicate, ct);

        public async Task<ModuleContent?> GetByIdAsync(object id, CancellationToken ct = default)
        {
            if (id is not int contentId) return null;
            return await context.ModuleContents.FirstOrDefaultAsync(x => x.Id == contentId && !x.Deleted, ct);
        }

        public async Task<IReadOnlyList<ModuleContent>> ListAsync(CancellationToken ct = default)
            => await context.ModuleContents.Where(x => !x.Deleted).ToListAsync(ct);

        public async Task<IReadOnlyList<ModuleContent>> ListAsync(Expression<Func<ModuleContent, bool>> predicate, CancellationToken ct = default)
            => await context.ModuleContents.Where(x => !x.Deleted).Where(predicate).ToListAsync(ct);

        public async Task<ModuleContent?> SingleOrDefaultAsync(Expression<Func<ModuleContent, bool>> predicate, CancellationToken ct = default)
            => await context.ModuleContents.Where(x => !x.Deleted).SingleOrDefaultAsync(predicate, ct);

        public void Update(ModuleContent entity) => context.ModuleContents.Update(entity);

        public async Task<IReadOnlyList<ModuleContent>> GetByModuleAsync(int moduleId, CancellationToken ct = default)
            => await context.ModuleContents
                .Where(x => x.ModuleId == moduleId && !x.Deleted)
                .OrderBy(x => x.OrderIndex)
                .ToListAsync(ct);

        public async Task<int> GetNextOrderIndexAsync(int moduleId, CancellationToken ct = default)
        {
            var max = await context.ModuleContents
                .Where(x => x.ModuleId == moduleId && !x.Deleted)
                .MaxAsync(x => (int?)x.OrderIndex, ct);
            return (max ?? 0) + 1;
        }
    }
}
