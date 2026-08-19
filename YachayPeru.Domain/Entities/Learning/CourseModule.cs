using YachayPeru.Domain.Base;

namespace YachayPeru.Domain.Entities.Learning
{
    public class CourseModule : BaseEntity
    {
        public int CourseVersionId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int OrderIndex { get; set; }
        public decimal? DurationHours { get; set; }
    }
}
