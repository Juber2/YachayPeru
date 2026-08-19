using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Aprendiz.Noticias.Queries.GetNoticiaById
{
    public record GetNoticiaByIdQuery(int Id) : IRequest<Result<AprendizNoticiaDetail>>;

    public class AprendizNoticiaDetail
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
