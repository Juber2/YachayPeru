namespace YachayPeru.API.Contracts.Administration.Biblioteca.Response
{
    public record MediaItemListItemResponse
    {
        public int Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public string MediaTypeCode { get; init; } = string.Empty;
        public int RegionId { get; init; }
        public string RegionTitle { get; init; } = string.Empty;
        public string? ThumbnailUrl { get; init; }
        public bool IsActive { get; init; }
        public DateTime CreatedAt { get; init; }
    }

    public record MediaItemDetailResponse
    {
        public int Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public string MediaTypeCode { get; init; } = string.Empty;
        public int RegionId { get; init; }
        public string RegionTitle { get; init; } = string.Empty;
        public string? ThumbnailUrl { get; init; }
        public string? ExternalUrl { get; init; }
        public string? LegendText { get; init; }
        public bool IsActive { get; init; }
    }
}
