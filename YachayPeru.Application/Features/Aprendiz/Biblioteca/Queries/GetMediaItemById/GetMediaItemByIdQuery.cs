using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Aprendiz.Biblioteca.Queries.GetMediaItemById
{
    public sealed record GetMediaItemByIdQuery(int Id) : IRequest<Result<AprendizMediaItemDetail>>;

    public sealed record AprendizMediaItemDetail
    {
        public int Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public string MediaTypeCode { get; init; } = string.Empty;
        public int RegionId { get; init; }
        public string RegionTitle { get; init; } = string.Empty;
        public string? ThumbnailUrl { get; init; }
        public string? ExternalUrl { get; init; }
        public string? LegendText { get; init; }
        public bool IsPlayable { get; init; }
    }
}
