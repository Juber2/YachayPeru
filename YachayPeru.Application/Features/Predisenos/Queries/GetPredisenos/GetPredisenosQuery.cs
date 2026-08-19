using MediatR;

namespace YachayPeru.Application.Features.Predisenos.Queries.GetPredisenos
{
    public record GetPredisenosQuery : IRequest<IReadOnlyList<PredisenoListItem>>;

    public class PredisenoListItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string TreeJson { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
