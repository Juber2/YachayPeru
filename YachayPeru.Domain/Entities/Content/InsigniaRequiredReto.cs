using YachayPeru.Domain.Base;

namespace YachayPeru.Domain.Entities.Content
{
    public class InsigniaRequiredReto : BaseEntity
    {
        public int InsigniaId { get; set; }
        public int RetoId { get; set; }
    }
}
