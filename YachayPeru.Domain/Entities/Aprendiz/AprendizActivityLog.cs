using YachayPeru.Domain.Base;

namespace YachayPeru.Domain.Entities.Aprendiz
{
    public class AprendizActivityLog : BaseEntity
    {
        public int UserId { get; set; }
        public string Text { get; set; } = string.Empty;
        public int? RegionId { get; set; }
    }
}
