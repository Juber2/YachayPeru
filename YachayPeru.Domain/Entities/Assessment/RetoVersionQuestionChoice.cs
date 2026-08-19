using YachayPeru.Domain.Base;

namespace YachayPeru.Domain.Entities.Assessment
{
    public class RetoVersionQuestionChoice : BaseEntity
    {
        public int RetoVersionQuestionId { get; set; }
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public int OrderIndex { get; set; }
    }
}
