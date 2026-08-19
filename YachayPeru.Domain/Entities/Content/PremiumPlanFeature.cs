using YachayPeru.Domain.Base;

namespace YachayPeru.Domain.Entities.Content
{
    /// <summary>Tabla puente: qué PremiumBenefit aplica a cada PremiumPlan.</summary>
    public class PremiumPlanFeature : BaseEntity
    {
        public int PlanId { get; set; }
        public PremiumPlan Plan { get; set; } = null!;
        public int BenefitId { get; set; }
        public PremiumBenefit Benefit { get; set; } = null!;
    }
}
