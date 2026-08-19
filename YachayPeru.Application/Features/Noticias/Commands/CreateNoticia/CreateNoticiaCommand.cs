using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Noticias.Commands.CreateNoticia
{
    public sealed record CreateNoticiaCommand : IRequest<Result<int>>
    {
        public string Title { get; init; } = default!;
        public string Category { get; init; } = default!;
        public string Body { get; init; } = default!;
        public bool IsActive { get; init; } = true;
    }
}
