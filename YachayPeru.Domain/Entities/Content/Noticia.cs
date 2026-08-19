using YachayPeru.Domain.Base;

namespace YachayPeru.Domain.Entities.Content
{
    public class Noticia : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
