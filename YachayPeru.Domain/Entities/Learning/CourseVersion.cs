using YachayPeru.Domain.Base;

namespace YachayPeru.Domain.Entities.Learning
{
    public class CourseVersion : BaseEntity
    {
        public int CourseId { get; set; }
        public int VersionNumber { get; set; }
        public string StatusCode { get; set; } = string.Empty;
        public decimal? DurationHours { get; set; }
        public bool IsCurrent { get; set; }
        public DateTime? PublishedAt { get; set; }
        public int? PublishedBy { get; set; }
    }
}
