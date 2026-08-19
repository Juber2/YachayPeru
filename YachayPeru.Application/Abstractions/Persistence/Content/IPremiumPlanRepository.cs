using YachayPeru.Domain.Entities.Content;

namespace YachayPeru.Application.Abstractions.Persistence.Content
{
    public interface IPremiumPlanRepository : IRepository<PremiumPlan>
    {
        Task<IReadOnlyList<int>> GetFeatureBenefitIdsAsync(int planId, CancellationToken ct = default);

        /// <summary>Borra y reinserta las filas de PremiumPlanFeature del plan indicado (delete-and-reinsert).</summary>
        Task ReplacePlanFeaturesAsync(int planId, IReadOnlyCollection<int> benefitIds, CancellationToken ct = default);

        /// <summary>True si hay alguna PremiumWaitlistEntry apuntando a este plan — usado para bloquear el delete.</summary>
        Task<bool> IsUsedInWaitlistAsync(int planId, CancellationToken ct = default);
    }
}
