using YachayPeru.Domain.Base;

namespace YachayPeru.Domain.Entities.Learning
{
    public class Course : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public int? SourceTemplateId { get; set; }
        public string? CoverImageUrl { get; set; }
        public string? ZoneCode { get; set; }
    }
}
