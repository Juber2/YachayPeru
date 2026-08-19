using YachayPeru.Domain.Entities.Content;

namespace YachayPeru.Application.Abstractions.Persistence.Content
{
    public interface IPremiumBenefitRepository : IRepository<PremiumBenefit>
    {
        /// <summary>True si el beneficio está asignado a algún plan (Free y/o Premium) — usado para bloquear el delete.</summary>
        Task<bool> IsUsedInAnyPlanAsync(int benefitId, CancellationToken ct = default);
    }
}
