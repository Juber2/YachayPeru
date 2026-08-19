using Microsoft.EntityFrameworkCore;
using YachayPeru.Application.Abstractions.Persistence.Learning;
using YachayPeru.Domain.Entities.Learning;
using YachayPeru.Infrastructure.Persistence.SqlServer;
using System.Linq.Expressions;

namespace YachayPeru.Infrastructure.Persistence.Repositories
{
    public class ModuleContentFileRepository : IModuleContentFileRepository
    {
        private readonly ApplicationDbContext context;

        public ModuleContentFileRepository(ApplicationDbContext _context) => context = _context;

        public async Task<ModuleContentFile> AddAsync(ModuleContentFile entity, CancellationToken ct = default)
        {
            await context.ModuleContentFiles.AddAsync(entity, ct);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<ModuleContentFile> entities, CancellationToken ct = default)
            => await context.ModuleContentFiles.AddRangeAsync(entities, ct);

        public async Task<bool> AnyAsync(Expression<Func<ModuleContentFile, bool>> predicate, CancellationToken ct = default)
            => await context.ModuleContentFiles.Where(x => !x.Deleted).AnyAsync(predicate, ct);

        public async Task<int> CountAsync(Expression<Func<ModuleContentFile, bool>>? predicate = null, CancellationToken ct = default)
        {
            var q = context.ModuleContentFiles.Where(x => !x.Deleted);
            if (predicate is not null) q = q.Where(predicate);
            return await q.CountAsync(ct);
        }

        public void Delete(ModuleContentFile entity) => context.ModuleContentFiles.Remove(entity);
        public void DeleteRange(IEnumerable<ModuleContentFile> entities) => context.ModuleContentFiles.RemoveRange(entities);

        public async Task<ModuleContentFile?> FirstOrDefaultAsync(Expression<Func<ModuleContentFile, bool>> predicate, CancellationToken ct = default)
            => await context.ModuleContentFiles.Where(x => !x.Deleted).FirstOrDefaultAsync(predicate, ct);

        public async Task<ModuleContentFile?> GetByIdAsync(object id, CancellationToken ct = default)
        {
            if (id is not int fileId) return null;
            return await context.ModuleContentFiles.FirstOrDefaultAsync(x => x.Id == fileId && !x.Deleted, ct);
        }

        public async Task<IReadOnlyList<ModuleContentFile>> ListAsync(CancellationToken ct = default)
            => await context.ModuleContentFiles.Where(x => !x.Deleted).ToListAsync(ct);

        public async Task<IReadOnlyList<ModuleContentFile>> ListAsync(Expression<Func<ModuleContentFile, bool>> predicate, CancellationToken ct = default)
            => await context.ModuleContentFiles.Where(x => !x.Deleted).Where(predicate).ToListAsync(ct);

        public async Task<ModuleContentFile?> SingleOrDefaultAsync(Expression<Func<ModuleContentFile, bool>> predicate, CancellationToken ct = default)
            => await context.ModuleContentFiles.Where(x => !x.Deleted).SingleOrDefaultAsync(predicate, ct);

        public void Update(ModuleContentFile entity) => context.ModuleContentFiles.Update(entity);

        public async Task<IReadOnlyList<ModuleContentFile>> GetByItemAsync(int moduleContentId, CancellationToken ct = default)
            => await context.ModuleContentFiles
                .Where(x => x.ModuleContentId == moduleContentId && !x.Deleted)
                .OrderBy(x => x.OrderIndex)
                .ToListAsync(ct);
    }
}
