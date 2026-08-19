using MediatR;

namespace YachayPeru.Application.Features.Aprendiz.Biblioteca.Queries.GetMediaItems
{
    public sealed record GetMediaItemsQuery(string? MediaTypeCode, int? RegionId) : IRequest<IReadOnlyList<AprendizMediaItemListItem>>;

    public sealed record AprendizMediaItemListItem
    {
        public int Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public string MediaTypeCode { get; init; } = string.Empty;
        public int RegionId { get; init; }
        public string RegionTitle { get; init; } = string.Empty;
        public string? ThumbnailUrl { get; init; }
        public bool IsPlayable { get; init; }
    }
}
