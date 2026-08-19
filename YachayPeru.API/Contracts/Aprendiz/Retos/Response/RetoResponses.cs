namespace YachayPeru.API.Contracts.Aprendiz.Retos.Response
{
    public record AprendizRetoListItemResponse
    {
        public int Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public int RegionId { get; init; }
        public string RegionTitle { get; init; } = string.Empty;
        public int QuestionCount { get; init; }
        public decimal TotalPoints { get; init; }
        public decimal EarnedPoints { get; init; }
        public bool IsCompleted { get; init; }
        public int AttemptsUsed { get; init; }
        public int? MaxAttempts { get; init; }
    }

    public record AprendizChoiceResponse
    {
        public int Id { get; init; }
        public string Text { get; init; } = string.Empty;
        public int OrderIndex { get; init; }
    }

    public record AprendizQuestionResponse
    {
        public int Id { get; init; }
        public string QuestionTypeCode { get; init; } = string.Empty;
        public string QuestionText { get; init; } = string.Empty;
        public decimal Points { get; init; }
        public int OrderIndex { get; init; }
        public List<AprendizChoiceResponse> Choices { get; init; } = [];
        public int BlanksCount { get; init; }
    }

    public record AprendizRetoPlayResponse
    {
        public int Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public int? TimeLimitMinutes { get; init; }
        public decimal PassingScore { get; init; }
        public bool ShuffleQuestionOrder { get; init; }
        public bool ShuffleOptionOrder { get; init; }
        public List<AprendizQuestionResponse> Questions { get; init; } = [];
    }

    public record PerQuestionResultResponse
    {
        public int QuestionId { get; init; }
        public bool IsCorrect { get; init; }
        public List<int>? CorrectChoiceIds { get; init; }
        public List<string>? CorrectBlankAnswers { get; init; }
    }

    public record RetoAttemptResultResponse
    {
        public decimal EarnedPoints { get; init; }
        public decimal TotalPoints { get; init; }
        public bool Passed { get; init; }
        public int CorrectCount { get; init; }
        public int TotalQuestions { get; init; }
        public List<PerQuestionResultResponse> PerQuestion { get; init; } = [];
    }
}
