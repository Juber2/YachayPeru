using MediatR;

namespace YachayPeru.Application.Features.Aprendiz.Noticias.Queries.GetNoticias
{
    public record GetNoticiasQuery : IRequest<IReadOnlyList<AprendizNoticiaListItem>>;

    public class AprendizNoticiaListItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string Body { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
