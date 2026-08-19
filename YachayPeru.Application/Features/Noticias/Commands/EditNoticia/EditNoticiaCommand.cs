using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Noticias.Commands.EditNoticia
{
    public sealed record EditNoticiaCommand : IRequest<Result>
    {
        public int Id { get; init; }
        public string Title { get; init; } = default!;
        public string Category { get; init; } = default!;
        public string Body { get; init; } = default!;
        public bool IsActive { get; init; }
    }
}
