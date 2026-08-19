namespace YachayPeru.API.Contracts.Retos.Response
{
    public record RetoListItemResponse
    {
        public int Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public string StatusCode { get; init; } = string.Empty;
        public int QuestionCount { get; init; }
        public decimal TotalPoints { get; init; }
        public DateTime CreatedAt { get; init; }
    }

    public record RetoLookupItemResponse
    {
        public int Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public int CourseId { get; init; }
        public string RegionTitle { get; init; } = string.Empty;
    }

    public record RetoResponse
    {
        public int Id { get; init; }
        public int VersionNumber { get; init; }
        public string StatusCode { get; init; } = string.Empty;
        public string Title { get; init; } = string.Empty;
        public decimal PassingScore { get; init; }
        public int? TimeLimitMinutes { get; init; }
        public int MaxAttempts { get; init; }
        public bool ShuffleQuestionOrder { get; init; }
        public bool ShuffleOptionOrder { get; init; }
        public bool ShowResultsAtEnd { get; init; }
        public List<QuestionResponse> Questions { get; init; } = [];
    }

    public record QuestionResponse
    {
        public int Id { get; init; }
        public string QuestionTypeCode { get; init; } = string.Empty;
        public string QuestionText { get; init; } = string.Empty;
        public decimal Points { get; init; }
        public int OrderIndex { get; init; }
        public List<ChoiceResponse> Choices { get; init; } = [];
    }

    public record ChoiceResponse
    {
        public int Id { get; init; }
        public string Text { get; init; } = string.Empty;
        public bool IsCorrect { get; init; }
        public int OrderIndex { get; init; }
    }

    public record RetoVersionSummaryResponse
    {
        public int Id { get; init; }
        public int VersionNumber { get; init; }
        public string StatusCode { get; init; } = string.Empty;
        public string Title { get; init; } = string.Empty;
        public DateTime CreatedAt { get; init; }
    }

    public record RetoVersionDetailResponse
    {
        public int Id { get; init; }
        public int VersionNumber { get; init; }
        public string StatusCode { get; init; } = string.Empty;
        public string Title { get; init; } = string.Empty;
        public decimal PassingScore { get; init; }
        public int? TimeLimitMinutes { get; init; }
        public int MaxAttempts { get; init; }
        public bool ShuffleQuestionOrder { get; init; }
        public bool ShuffleOptionOrder { get; init; }
        public bool ShowResultsAtEnd { get; init; }
        public DateTime CreatedAt { get; init; }
        public List<QuestionResponse> Questions { get; init; } = [];
    }
}
