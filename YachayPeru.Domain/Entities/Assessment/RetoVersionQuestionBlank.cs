using YachayPeru.Domain.Base;

namespace YachayPeru.Domain.Entities.Assessment
{
    public class RetoVersionQuestionBlank : BaseEntity
    {
        public int RetoVersionQuestionId { get; set; }
        public int BlankIndex { get; set; }
        public string CorrectAnswer { get; set; } = string.Empty;
        public int OrderIndex { get; set; }
    }
}
