using YachayPeru.Domain.Base;

namespace YachayPeru.Domain.Entities.Content
{
    /// <summary>Beneficio de texto libre reutilizable, asignable al plan Free y/o Premium.</summary>
    public class PremiumBenefit : BaseEntity
    {
        public string Code { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
