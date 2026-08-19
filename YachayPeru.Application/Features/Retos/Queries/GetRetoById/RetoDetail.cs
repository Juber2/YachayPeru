using YachayPeru.Application.Features.Retos.Queries.GetRetoById;

namespace YachayPeru.Application.Actions.Courses
{
    public class RetoVersionSummary
    {
        public int Id { get; set; }
        public int VersionNumber { get; set; }
        public string StatusCode { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class RetoVersionDetail
    {
        public int Id { get; set; }
        public int VersionNumber { get; set; }
        public string StatusCode { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public decimal PassingScore { get; set; }
        public int? TimeLimitMinutes { get; set; }
        public int MaxAttempts { get; set; }
        public bool ShuffleQuestionOrder { get; set; }
        public bool ShuffleOptionOrder { get; set; }
        public bool ShowResultsAtEnd { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<QuestionDto> Questions { get; set; } = [];
    }
}

namespace YachayPeru.Application.Features.Retos.Queries.GetRetoById
{
    public class RetoDetail
    {
        public int Id { get; set; }
        public int VersionNumber { get; set; }
        public string StatusCode { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public decimal PassingScore { get; set; }
        public int? TimeLimitMinutes { get; set; }
        public int MaxAttempts { get; set; }
        public bool ShuffleQuestionOrder { get; set; }
        public bool ShuffleOptionOrder { get; set; }
        public bool ShowResultsAtEnd { get; set; }
        public List<QuestionDto> Questions { get; set; } = [];
    }

    public class QuestionDto
    {
        public int Id { get; set; }
        public string QuestionTypeCode { get; set; } = string.Empty;
        public string QuestionText { get; set; } = string.Empty;
        public decimal Points { get; set; }
        public int OrderIndex { get; set; }
        public List<ChoiceDto> Choices { get; set; } = [];
    }

    public class ChoiceDto
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public int OrderIndex { get; set; }
    }
}
