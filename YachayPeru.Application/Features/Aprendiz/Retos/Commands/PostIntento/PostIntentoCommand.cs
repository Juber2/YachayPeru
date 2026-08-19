using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Aprendiz.Retos.Commands.PostIntento
{
    public sealed record PostIntentoCommand : IRequest<Result<RetoAttemptResult>>
    {
        public int UserId { get; init; }
        public int RetoId { get; init; }
        public List<AnswerEntry> Answers { get; init; } = [];
    }

    public class AnswerEntry
    {
        public int QuestionId { get; set; }
        public List<int>? SelectedChoiceIds { get; set; }
        public List<string>? BlankAnswers { get; set; }
    }

    public class RetoAttemptResult
    {
        public decimal EarnedPoints { get; set; }
        public decimal TotalPoints { get; set; }
        public bool Passed { get; set; }
        public int CorrectCount { get; set; }
        public int TotalQuestions { get; set; }
        public List<PerQuestionResult> PerQuestion { get; set; } = [];
    }

    public class PerQuestionResult
    {
        public int QuestionId { get; set; }
        public bool IsCorrect { get; set; }
        public List<int>? CorrectChoiceIds { get; set; }
        public List<string>? CorrectBlankAnswers { get; set; }
    }
}
