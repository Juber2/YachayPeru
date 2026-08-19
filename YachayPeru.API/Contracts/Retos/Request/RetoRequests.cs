namespace YachayPeru.API.Contracts.Retos.Request
{
    public class UpsertRetoSettingsRequest
    {
        public string Title { get; set; } = default!;
        public decimal PassingScore { get; set; }
        public int? TimeLimitMinutes { get; set; }
        public int MaxAttempts { get; set; } = 3;
        public bool ShuffleQuestionOrder { get; set; }
        public bool ShuffleOptionOrder { get; set; }
        public bool ShowResultsAtEnd { get; set; } = true;
    }

    public class AddRetoQuestionRequest
    {
        public string QuestionTypeCode { get; set; } = string.Empty;
        public string QuestionText { get; set; } = default!;
        public decimal Points { get; set; }
        public List<QuestionChoiceRequest> Choices { get; set; } = [];
        public List<QuestionBlankRequest> Blanks { get; set; } = [];
    }

    public class EditRetoQuestionRequest
    {
        public string QuestionText { get; set; } = default!;
        public decimal Points { get; set; }
        public List<QuestionChoiceRequest> Choices { get; set; } = [];
        public List<QuestionBlankRequest> Blanks { get; set; } = [];
    }

    public class QuestionChoiceRequest
    {
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public int OrderIndex { get; set; }
    }

    public class QuestionBlankRequest
    {
        public int BlankIndex { get; set; }
        public string CorrectAnswer { get; set; } = string.Empty;
        public int OrderIndex { get; set; }
    }

    public class ReorderQuestionsRequest
    {
        public List<QuestionOrderEntryRequest> Items { get; set; } = [];
    }

    public class QuestionOrderEntryRequest
    {
        public int QuestionId { get; set; }
        public int OrderIndex { get; set; }
    }
}
