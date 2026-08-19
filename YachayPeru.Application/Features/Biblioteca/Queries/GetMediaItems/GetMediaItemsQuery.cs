using MediatR;

namespace YachayPeru.Application.Features.Biblioteca.Queries.GetMediaItems
{
    public record GetMediaItemsQuery : IRequest<IReadOnlyList<MediaItemListItem>>;

    public class MediaItemListItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string MediaTypeCode { get; set; } = string.Empty;
        public int RegionId { get; set; }
        public string RegionTitle { get; set; } = string.Empty;
        public string? ThumbnailUrl { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
