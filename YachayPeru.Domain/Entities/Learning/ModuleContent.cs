using YachayPeru.Domain.Base;

namespace YachayPeru.Domain.Entities.Learning
{
    public class ModuleContent : BaseEntity
    {
        public int ModuleId { get; set; }
        public string? Text { get; set; }
        public string? DesignJson { get; set; }
        public int OrderIndex { get; set; }
    }
}
