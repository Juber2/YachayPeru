namespace YachayPeru.Application.Actions.Courses.Models
{
    public class UpsertRetoSettingsInput
    {
        public int RetoId { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal PassingScore { get; set; }
        public int? TimeLimitMinutes { get; set; }
        public int MaxAttempts { get; set; } = 3;
        public bool ShuffleQuestionOrder { get; set; }
        public bool ShuffleOptionOrder { get; set; }
        public bool ShowResultsAtEnd { get; set; } = true;
    }

    public class AddRetoQuestionInput
    {
        public int RetoId { get; set; }
        public string QuestionTypeCode { get; set; } = string.Empty;
        public string QuestionText { get; set; } = string.Empty;
        public decimal Points { get; set; }
        public List<QuestionChoiceInput> Choices { get; set; } = [];
        public List<QuestionBlankInput> Blanks { get; set; } = [];
    }

    public class EditRetoQuestionInput
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public decimal Points { get; set; }
        public List<QuestionChoiceInput> Choices { get; set; } = [];
        public List<QuestionBlankInput> Blanks { get; set; } = [];
    }

    public class QuestionChoiceInput
    {
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public int OrderIndex { get; set; }
    }

    public class QuestionBlankInput
    {
        public int BlankIndex { get; set; }
        public string CorrectAnswer { get; set; } = string.Empty;
        public int OrderIndex { get; set; }
    }

    public class ReorderQuestionsInput
    {
        public int RetoId { get; set; }
        public List<QuestionOrderItem> Items { get; set; } = [];
    }

    public class QuestionOrderItem
    {
        public int QuestionId { get; set; }
        public int OrderIndex { get; set; }
    }
}
