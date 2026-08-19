using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Biblioteca.Commands.EditMediaItem
{
    public sealed record EditMediaItemCommand : IRequest<Result>
    {
        public int Id { get; init; }
        public string Title { get; init; } = default!;
        public string MediaTypeCode { get; init; } = default!;
        public int RegionId { get; init; }
        public string? ExternalUrl { get; init; }
        public string? LegendText { get; init; }
        public bool IsActive { get; init; }
    }
}
