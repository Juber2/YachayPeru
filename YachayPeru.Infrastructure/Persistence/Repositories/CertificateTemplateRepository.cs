using Microsoft.EntityFrameworkCore;
using YachayPeru.Application.Abstractions.Persistence.Learning;
using YachayPeru.Domain.Entities.Learning;
using YachayPeru.Infrastructure.Persistence.SqlServer;
using System.Linq.Expressions;

namespace YachayPeru.Infrastructure.Persistence.Repositories
{
    public class CertificateTemplateRepository : ICertificateTemplateRepository
    {
        private readonly ApplicationDbContext context;

        public CertificateTemplateRepository(ApplicationDbContext _context) => context = _context;

        public async Task<CertificateTemplate> AddAsync(CertificateTemplate entity, CancellationToken ct = default)
        {
            await context.CertificateTemplates.AddAsync(entity, ct);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<CertificateTemplate> entities, CancellationToken ct = default)
            => await context.CertificateTemplates.AddRangeAsync(entities, ct);

        public async Task<bool> AnyAsync(Expression<Func<CertificateTemplate, bool>> predicate, CancellationToken ct = default)
            => await context.CertificateTemplates.Where(x => !x.Deleted).AnyAsync(predicate, ct);

        public async Task<int> CountAsync(Expression<Func<CertificateTemplate, bool>>? predicate = null, CancellationToken ct = default)
        {
            var q = context.CertificateTemplates.Where(x => !x.Deleted);
            if (predicate is not null) q = q.Where(predicate);
            return await q.CountAsync(ct);
        }

        public void Delete(CertificateTemplate entity) => context.CertificateTemplates.Remove(entity);
        public void DeleteRange(IEnumerable<CertificateTemplate> entities) => context.CertificateTemplates.RemoveRange(entities);

        public async Task<CertificateTemplate?> FirstOrDefaultAsync(Expression<Func<CertificateTemplate, bool>> predicate, CancellationToken ct = default)
            => await context.CertificateTemplates.Where(x => !x.Deleted).FirstOrDefaultAsync(predicate, ct);

        public async Task<CertificateTemplate?> GetByIdAsync(object id, CancellationToken ct = default)
        {
            if (id is not int certId) return null;
            return await context.CertificateTemplates.FirstOrDefaultAsync(x => x.Id == certId && !x.Deleted, ct);
        }

        public async Task<IReadOnlyList<CertificateTemplate>> ListAsync(CancellationToken ct = default)
            => await context.CertificateTemplates.Where(x => !x.Deleted).ToListAsync(ct);

        public async Task<IReadOnlyList<CertificateTemplate>> ListAsync(Expression<Func<CertificateTemplate, bool>> predicate, CancellationToken ct = default)
            => await context.CertificateTemplates.Where(x => !x.Deleted).Where(predicate).ToListAsync(ct);

        public async Task<CertificateTemplate?> SingleOrDefaultAsync(Expression<Func<CertificateTemplate, bool>> predicate, CancellationToken ct = default)
            => await context.CertificateTemplates.Where(x => !x.Deleted).SingleOrDefaultAsync(predicate, ct);

        public void Update(CertificateTemplate entity) => context.CertificateTemplates.Update(entity);

        public async Task<CertificateTemplate?> GetByRetoAsync(int retoId, CancellationToken ct = default)
            => await context.CertificateTemplates
                .FirstOrDefaultAsync(x => x.RetoId == retoId && !x.Deleted, ct);
    }
}
