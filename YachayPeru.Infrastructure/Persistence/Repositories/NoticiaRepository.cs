using Microsoft.EntityFrameworkCore;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Domain.Entities.Content;
using YachayPeru.Infrastructure.Persistence.SqlServer;
using System.Linq.Expressions;

namespace YachayPeru.Infrastructure.Persistence.Repositories
{
    public class NoticiaRepository : INoticiaRepository
    {
        private readonly ApplicationDbContext context;

        public NoticiaRepository(ApplicationDbContext _context) => context = _context;

        public async Task<Noticia> AddAsync(Noticia entity, CancellationToken ct = default)
        {
            await context.Noticias.AddAsync(entity, ct);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<Noticia> entities, CancellationToken ct = default)
            => await context.Noticias.AddRangeAsync(entities, ct);

        public async Task<bool> AnyAsync(Expression<Func<Noticia, bool>> predicate, CancellationToken ct = default)
            => await context.Noticias.Where(x => !x.Deleted).AnyAsync(predicate, ct);

        public async Task<int> CountAsync(Expression<Func<Noticia, bool>>? predicate = null, CancellationToken ct = default)
        {
            var q = context.Noticias.Where(x => !x.Deleted);
            if (predicate is not null) q = q.Where(predicate);
            return await q.CountAsync(ct);
        }

        public void Delete(Noticia entity) => context.Noticias.Remove(entity);
        public void DeleteRange(IEnumerable<Noticia> entities) => context.Noticias.RemoveRange(entities);

        public async Task<Noticia?> FirstOrDefaultAsync(Expression<Func<Noticia, bool>> predicate, CancellationToken ct = default)
            => await context.Noticias.Where(x => !x.Deleted).FirstOrDefaultAsync(predicate, ct);

        public async Task<Noticia?> GetByIdAsync(object id, CancellationToken ct = default)
        {
            if (id is not int nid) return null;
            return await context.Noticias.FirstOrDefaultAsync(x => x.Id == nid && !x.Deleted, ct);
        }

        public async Task<IReadOnlyList<Noticia>> ListAsync(CancellationToken ct = default)
            => await context.Noticias.Where(x => !x.Deleted).ToListAsync(ct);

        public async Task<IReadOnlyList<Noticia>> ListAsync(Expression<Func<Noticia, bool>> predicate, CancellationToken ct = default)
            => await context.Noticias.Where(x => !x.Deleted).Where(predicate).ToListAsync(ct);

        public async Task<Noticia?> SingleOrDefaultAsync(Expression<Func<Noticia, bool>> predicate, CancellationToken ct = default)
            => await context.Noticias.Where(x => !x.Deleted).SingleOrDefaultAsync(predicate, ct);

        public void Update(Noticia entity) => context.Noticias.Update(entity);
    }
}
