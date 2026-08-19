using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Noticias.Queries.GetNoticiaById
{
    public record GetNoticiaByIdQuery(int Id) : IRequest<Result<NoticiaDetail>>;

    public class NoticiaDetail
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; }
    }
}
