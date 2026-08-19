using YachayPeru.Domain.Base;

namespace YachayPeru.Domain.Entities.Content
{
    public class RegionDestacada : BaseEntity
    {
        public int CourseId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
