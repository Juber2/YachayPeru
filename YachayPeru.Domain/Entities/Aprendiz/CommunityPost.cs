using YachayPeru.Domain.Base;

namespace YachayPeru.Domain.Entities.Aprendiz
{
    public class CommunityPost : BaseEntity
    {
        public string AuthorName { get; set; } = string.Empty;
        public int? RegionId { get; set; }
        public string Text { get; set; } = string.Empty;
        public string? PhotoUrl { get; set; }
    }
}
