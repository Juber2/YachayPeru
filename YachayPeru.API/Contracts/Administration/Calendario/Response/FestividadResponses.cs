namespace YachayPeru.API.Contracts.Administration.Calendario.Response
{
    public record FestividadListItemResponse
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public int RegionId { get; init; }
        public string RegionTitle { get; init; } = string.Empty;
        public int Month { get; init; }
        public int Day { get; init; }
        public bool IsActive { get; init; }
        public DateTime CreatedAt { get; init; }
    }

    public record FestividadDetailResponse
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string? Description { get; init; }
        public int RegionId { get; init; }
        public string RegionTitle { get; init; } = string.Empty;
        public int Month { get; init; }
        public int Day { get; init; }
        public bool IsActive { get; init; }
    }
}
