using YachayPeru.Domain.Base;

namespace YachayPeru.Domain.Entities.Aprendiz
{
    public class PremiumWaitlistEntry : BaseEntity
    {
        public int UserId { get; set; }
        public int PlanId { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string? ReceiptUrl { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? RejectionReason { get; set; }
        public bool ReviewSeen { get; set; } = true;
        public DateTime JoinedAt { get; set; }
    }
}
