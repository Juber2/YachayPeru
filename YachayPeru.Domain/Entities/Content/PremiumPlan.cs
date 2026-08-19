using YachayPeru.Domain.Base;

namespace YachayPeru.Domain.Entities.Content
{
    /// <summary>Plan de suscripción con nombre propio, definido libremente por el admin (N planes).</summary>
    public class PremiumPlan : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
