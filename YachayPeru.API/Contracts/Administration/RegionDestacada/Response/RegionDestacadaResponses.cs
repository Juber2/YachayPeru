namespace YachayPeru.API.Contracts.Administration.RegionDestacada.Response
{
    public record RegionDestacadaListItemResponse
    {
        public int Id { get; init; }
        public int RegionId { get; init; }
        public string RegionTitle { get; init; } = string.Empty;
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
    }

    public record RegionDestacadaDetailResponse
    {
        public int Id { get; init; }
        public int RegionId { get; init; }
        public string RegionTitle { get; init; } = string.Empty;
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
    }
}
