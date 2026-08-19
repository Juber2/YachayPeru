using YachayPeru.Domain.Base;

namespace YachayPeru.Domain.Entities.Content
{
    public class Festividad : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int CourseId { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
