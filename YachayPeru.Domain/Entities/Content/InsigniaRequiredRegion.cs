using YachayPeru.Domain.Base;

namespace YachayPeru.Domain.Entities.Content
{
    public class InsigniaRequiredRegion : BaseEntity
    {
        public int InsigniaId { get; set; }
        public int CourseId { get; set; }
    }
}
