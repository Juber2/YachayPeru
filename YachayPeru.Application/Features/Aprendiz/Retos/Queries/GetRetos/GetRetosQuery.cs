using MediatR;

namespace YachayPeru.Application.Features.Aprendiz.Retos.Queries.GetRetos
{
    public record GetRetosQuery(int UserId, int? RegionId) : IRequest<IReadOnlyList<AprendizRetoListItem>>;

    public class AprendizRetoListItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int RegionId { get; set; }
        public string RegionTitle { get; set; } = string.Empty;
        public int QuestionCount { get; set; }
        public decimal TotalPoints { get; set; }
        public decimal EarnedPoints { get; set; }
        public bool IsCompleted { get; set; }
        public int AttemptsUsed { get; set; }
        public int? MaxAttempts { get; set; }
    }
}
