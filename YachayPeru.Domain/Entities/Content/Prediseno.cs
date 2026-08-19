using YachayPeru.Domain.Base;

namespace YachayPeru.Domain.Entities.Content
{
    public class Prediseno : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string TreeJson { get; set; } = string.Empty;
    }
}
