using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Biblioteca.Queries.GetMediaItemById
{
    public record GetMediaItemByIdQuery(int Id) : IRequest<Result<MediaItemDetail>>;

    public class MediaItemDetail
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string MediaTypeCode { get; set; } = string.Empty;
        public int RegionId { get; set; }
        public string RegionTitle { get; set; } = string.Empty;
        public string? ThumbnailUrl { get; set; }
        public string? ExternalUrl { get; set; }
        public string? LegendText { get; set; }
        public bool IsActive { get; set; }
    }
}
