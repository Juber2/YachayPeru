namespace YachayPeru.API.Contracts.Administration.Calendario.Request
{
    public class UpsertFestividadRequest
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public int RegionId { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
