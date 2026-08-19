using YachayPeru.Domain.Base;

namespace YachayPeru.Domain.Entities.Content
{
    public class MediaItem : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string MediaTypeCode { get; set; } = string.Empty;
        public int CourseId { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string? ExternalUrl { get; set; }
        public string? LegendText { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
