using MediatR;

namespace YachayPeru.Application.Features.Noticias.Queries.GetNoticias
{
    public record GetNoticiasQuery : IRequest<IReadOnlyList<NoticiaListItem>>;

    public class NoticiaListItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
