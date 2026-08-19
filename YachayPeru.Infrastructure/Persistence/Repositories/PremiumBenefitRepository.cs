using Microsoft.EntityFrameworkCore;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Domain.Entities.Content;
using YachayPeru.Infrastructure.Persistence.SqlServer;
using System.Linq.Expressions;

namespace YachayPeru.Infrastructure.Persistence.Repositories
{
    public class PremiumBenefitRepository : IPremiumBenefitRepository
    {
        private readonly ApplicationDbContext context;

        public PremiumBenefitRepository(ApplicationDbContext _context) => context = _context;

        public async Task<PremiumBenefit> AddAsync(PremiumBenefit entity, CancellationToken ct = default)
        {
            await context.PremiumBenefits.AddAsync(entity, ct);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<PremiumBenefit> entities, CancellationToken ct = default)
            => await context.PremiumBenefits.AddRangeAsync(entities, ct);

        public async Task<bool> AnyAsync(Expression<Func<PremiumBenefit, bool>> predicate, CancellationToken ct = default)
            => await context.PremiumBenefits.Where(x => !x.Deleted).AnyAsync(predicate, ct);

        public async Task<int> CountAsync(Expression<Func<PremiumBenefit, bool>>? predicate = null, CancellationToken ct = default)
        {
            var q = context.PremiumBenefits.Where(x => !x.Deleted);
            if (predicate is not null) q = q.Where(predicate);
            return await q.CountAsync(ct);
        }

        public void Delete(PremiumBenefit entity) => context.PremiumBenefits.Remove(entity);
        public void DeleteRange(IEnumerable<PremiumBenefit> entities) => context.PremiumBenefits.RemoveRange(entities);

        public async Task<PremiumBenefit?> FirstOrDefaultAsync(Expression<Func<PremiumBenefit, bool>> predicate, CancellationToken ct = default)
            => await context.PremiumBenefits.Where(x => !x.Deleted).FirstOrDefaultAsync(predicate, ct);

        public async Task<PremiumBenefit?> GetByIdAsync(object id, CancellationToken ct = default)
        {
            if (id is not int bid) return null;
            return await context.PremiumBenefits.FirstOrDefaultAsync(x => x.Id == bid && !x.Deleted, ct);
        }

        public async Task<IReadOnlyList<PremiumBenefit>> ListAsync(CancellationToken ct = default)
            => await context.PremiumBenefits.Where(x => !x.Deleted).ToListAsync(ct);

        public async Task<IReadOnlyList<PremiumBenefit>> ListAsync(Expression<Func<PremiumBenefit, bool>> predicate, CancellationToken ct = default)
            => await context.PremiumBenefits.Where(x => !x.Deleted).Where(predicate).ToListAsync(ct);

        public async Task<PremiumBenefit?> SingleOrDefaultAsync(Expression<Func<PremiumBenefit, bool>> predicate, CancellationToken ct = default)
            => await context.PremiumBenefits.Where(x => !x.Deleted).SingleOrDefaultAsync(predicate, ct);

        public void Update(PremiumBenefit entity) => context.PremiumBenefits.Update(entity);

        public async Task<bool> IsUsedInAnyPlanAsync(int benefitId, CancellationToken ct = default)
            => await context.PremiumPlanFeatures.Where(x => !x.Deleted).AnyAsync(x => x.BenefitId == benefitId, ct);
    }
}
