using YachayPeru.Domain.Base;

namespace YachayPeru.Domain.Entities.Assessment
{
    public class RetoVersionQuestion : BaseEntity
    {
        public int RetoVersionId { get; set; }
        public string QuestionTypeCode { get; set; } = string.Empty;
        public string QuestionText { get; set; } = string.Empty;
        public decimal Points { get; set; }
        public int OrderIndex { get; set; }
    }
}
