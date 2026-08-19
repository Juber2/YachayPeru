namespace YachayPeru.API.Contracts.Aprendiz.Biblioteca.Response
{
    public record AprendizMediaItemListItemResponse
    {
        public int Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public string MediaTypeCode { get; init; } = string.Empty;
        public int RegionId { get; init; }
        public string RegionTitle { get; init; } = string.Empty;
        public string? ThumbnailUrl { get; init; }
        public bool IsPlayable { get; init; }
    }

    public record AprendizMediaItemDetailResponse
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
