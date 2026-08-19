using Microsoft.EntityFrameworkCore;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Domain.Entities.Content;
using YachayPeru.Infrastructure.Persistence.SqlServer;
using System.Linq.Expressions;

namespace YachayPeru.Infrastructure.Persistence.Repositories
{
    public class PremiumPlanRepository : IPremiumPlanRepository
    {
        private readonly ApplicationDbContext context;

        public PremiumPlanRepository(ApplicationDbContext _context) => context = _context;

        public async Task<PremiumPlan> AddAsync(PremiumPlan entity, CancellationToken ct = default)
        {
            await context.PremiumPlans.AddAsync(entity, ct);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<PremiumPlan> entities, CancellationToken ct = default)
            => await context.PremiumPlans.AddRangeAsync(entities, ct);

        public async Task<bool> AnyAsync(Expression<Func<PremiumPlan, bool>> predicate, CancellationToken ct = default)
            => await context.PremiumPlans.Where(x => !x.Deleted).AnyAsync(predicate, ct);

        public async Task<int> CountAsync(Expression<Func<PremiumPlan, bool>>? predicate = null, CancellationToken ct = default)
        {
            var q = context.PremiumPlans.Where(x => !x.Deleted);
            if (predicate is not null) q = q.Where(predicate);
            return await q.CountAsync(ct);
        }

        public void Delete(PremiumPlan entity) => context.PremiumPlans.Remove(entity);
        public void DeleteRange(IEnumerable<PremiumPlan> entities) => context.PremiumPlans.RemoveRange(entities);

        public async Task<PremiumPlan?> FirstOrDefaultAsync(Expression<Func<PremiumPlan, bool>> predicate, CancellationToken ct = default)
            => await context.PremiumPlans.Where(x => !x.Deleted).FirstOrDefaultAsync(predicate, ct);

        public async Task<PremiumPlan?> GetByIdAsync(object id, CancellationToken ct = default)
        {
            if (id is not int pid) return null;
            return await context.PremiumPlans.FirstOrDefaultAsync(x => x.Id == pid && !x.Deleted, ct);
        }

        public async Task<IReadOnlyList<PremiumPlan>> ListAsync(CancellationToken ct = default)
            => await context.PremiumPlans.Where(x => !x.Deleted).ToListAsync(ct);

        public async Task<IReadOnlyList<PremiumPlan>> ListAsync(Expression<Func<PremiumPlan, bool>> predicate, CancellationToken ct = default)
            => await context.PremiumPlans.Where(x => !x.Deleted).Where(predicate).ToListAsync(ct);

        public async Task<PremiumPlan?> SingleOrDefaultAsync(Expression<Func<PremiumPlan, bool>> predicate, CancellationToken ct = default)
            => await context.PremiumPlans.Where(x => !x.Deleted).SingleOrDefaultAsync(predicate, ct);

        public void Update(PremiumPlan entity) => context.PremiumPlans.Update(entity);

        public async Task<IReadOnlyList<int>> GetFeatureBenefitIdsAsync(int planId, CancellationToken ct = default)
            => await context.PremiumPlanFeatures
                .Where(x => x.PlanId == planId && !x.Deleted)
                .Select(x => x.BenefitId)
                .ToListAsync(ct);

        public async Task ReplacePlanFeaturesAsync(int planId, IReadOnlyCollection<int> benefitIds, CancellationToken ct = default)
        {
            var existing = await context.PremiumPlanFeatures
                .Where(x => x.PlanId == planId)
                .ToListAsync(ct);
            context.PremiumPlanFeatures.RemoveRange(existing);

            var newRows = benefitIds.Select(benefitId => new PremiumPlanFeature
            {
                PlanId = planId,
                BenefitId = benefitId,
                CreatedAt = DateTime.UtcNow
            });
            await context.PremiumPlanFeatures.AddRangeAsync(newRows, ct);
        }

        public async Task<bool> IsUsedInWaitlistAsync(int planId, CancellationToken ct = default)
            => await context.PremiumWaitlistEntries.Where(x => !x.Deleted).AnyAsync(x => x.PlanId == planId, ct);
    }
}
