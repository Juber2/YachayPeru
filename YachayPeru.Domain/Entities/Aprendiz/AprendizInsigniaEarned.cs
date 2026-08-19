using YachayPeru.Domain.Base;

namespace YachayPeru.Domain.Entities.Aprendiz
{
    public class AprendizInsigniaEarned : BaseEntity
    {
        public int UserId { get; set; }
        public int InsigniaId { get; set; }
        public DateTime EarnedAt { get; set; }
    }
}
