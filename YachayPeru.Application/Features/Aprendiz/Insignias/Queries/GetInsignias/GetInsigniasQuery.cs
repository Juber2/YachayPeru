using MediatR;

namespace YachayPeru.Application.Features.Aprendiz.Insignias.Queries.GetInsignias
{
    public record GetInsigniasQuery(int UserId) : IRequest<IReadOnlyList<AprendizInsigniaListItem>>;

    public class AprendizInsigniaListItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public bool IsEarned { get; set; }
        public DateTime? EarnedAt { get; set; }
    }
}
