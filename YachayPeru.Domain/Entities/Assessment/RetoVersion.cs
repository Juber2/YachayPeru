using YachayPeru.Domain.Base;

namespace YachayPeru.Domain.Entities.Assessment
{
    public class RetoVersion : BaseEntity
    {
        public int RetoId { get; set; }
        public int VersionNumber { get; set; }
        public string StatusCode { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public decimal PassingScore { get; set; }
        public int? TimeLimitMinutes { get; set; }
        public int MaxAttempts { get; set; } = 3;
        public bool ShuffleQuestionOrder { get; set; }
        public bool ShuffleOptionOrder { get; set; }
        public bool ShowResultsAtEnd { get; set; } = true;
    }
}
