using YachayPeru.Domain.Base;

namespace YachayPeru.Domain.Entities.Aprendiz
{
    public class RetoAttempt : BaseEntity
    {
        public int UserId { get; set; }
        public int RetoId { get; set; }
        public int RetoVersionId { get; set; }
        public decimal EarnedPoints { get; set; }
        public decimal TotalPoints { get; set; }
        public bool Passed { get; set; }
        public int CorrectCount { get; set; }
        public int TotalQuestions { get; set; }
    }
}
