using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Retos.Commands.Questions
{
    public sealed record AddRetoQuestionCommand : IRequest<Result<int>>
    {
        public int RetoId { get; init; }
        public string QuestionTypeCode { get; init; } = string.Empty;
        public string QuestionText { get; init; } = default!;
        public decimal Points { get; init; }
        public List<QuestionChoiceEntry> Choices { get; init; } = [];
        public List<QuestionBlankEntry> Blanks { get; init; } = [];
    }

    public sealed record EditRetoQuestionCommand : IRequest<Result<int>>
    {
        public int QuestionId { get; init; }
        public string QuestionText { get; init; } = default!;
        public decimal Points { get; init; }
        public List<QuestionChoiceEntry> Choices { get; init; } = [];
        public List<QuestionBlankEntry> Blanks { get; init; } = [];
    }

    public record DeleteRetoQuestionCommand(int QuestionId) : IRequest<Result>;

    public sealed record ReorderRetoQuestionsCommand : IRequest<Result>
    {
        public int RetoId { get; init; }
        public List<QuestionOrderEntry> Items { get; init; } = [];
    }

    public class QuestionChoiceEntry
    {
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
        public int OrderIndex { get; set; }
    }

    public class QuestionBlankEntry
    {
        public int BlankIndex { get; set; }
        public string CorrectAnswer { get; set; } = string.Empty;
        public int OrderIndex { get; set; }
    }

    public class QuestionOrderEntry
    {
        public int QuestionId { get; set; }
        public int OrderIndex { get; set; }
    }
}
