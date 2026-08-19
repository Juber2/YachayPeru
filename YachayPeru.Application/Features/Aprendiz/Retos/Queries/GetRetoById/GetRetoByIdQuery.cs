using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Aprendiz.Retos.Queries.GetRetoById
{
    public record GetRetoByIdQuery(int RetoId) : IRequest<Result<AprendizRetoPlay>>;

    public class AprendizRetoPlay
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int? TimeLimitMinutes { get; set; }
        public decimal PassingScore { get; set; }
        public bool ShuffleQuestionOrder { get; set; }
        public bool ShuffleOptionOrder { get; set; }
        public List<AprendizQuestion> Questions { get; set; } = [];
    }

    public class AprendizQuestion
    {
        public int Id { get; set; }
        public string QuestionTypeCode { get; set; } = string.Empty;
        public string QuestionText { get; set; } = string.Empty;
        public decimal Points { get; set; }
        public int OrderIndex { get; set; }
        public List<AprendizChoice> Choices { get; set; } = [];
        public int BlanksCount { get; set; }
    }

    public class AprendizChoice
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public int OrderIndex { get; set; }
    }
}
