namespace YachayPeru.API.Contracts.Aprendiz.Calendario.Response
{
    public record AprendizFestividadListItemResponse
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string? Description { get; init; }
        public int RegionId { get; init; }
        public string RegionTitle { get; init; } = string.Empty;
        public int Month { get; init; }
        public int Day { get; init; }
        public bool IsReminderOn { get; init; }
    }

    public record AprendizProximaFestividadResponse
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string? Description { get; init; }
        public int RegionId { get; init; }
        public string RegionTitle { get; init; } = string.Empty;
        public int Month { get; init; }
        public int Day { get; init; }
        public int DaysUntil { get; init; }
        public bool IsReminderOn { get; init; }
    }
}
