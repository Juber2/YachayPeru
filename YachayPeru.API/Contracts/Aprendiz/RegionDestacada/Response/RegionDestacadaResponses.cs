namespace YachayPeru.API.Contracts.Aprendiz.RegionDestacada.Response
{
    public record AprendizRegionDestacadaResponse
    {
        public int RegionId { get; init; }
        public string RegionTitle { get; init; } = string.Empty;
        public string? RegionDescription { get; init; }
        public string? CoverImageUrl { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
    }
}
