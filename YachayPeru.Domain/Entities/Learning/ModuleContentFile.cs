using YachayPeru.Domain.Base;

namespace YachayPeru.Domain.Entities.Learning
{
    public class ModuleContentFile : BaseEntity
    {
        public int ModuleContentId { get; set; }
        public string FileTypeCode { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public int OrderIndex { get; set; }
    }
}
