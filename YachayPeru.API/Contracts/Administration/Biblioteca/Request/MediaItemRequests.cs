namespace YachayPeru.API.Contracts.Administration.Biblioteca.Request
{
    public class UpsertMediaItemRequest
    {
        public string Title { get; set; } = default!;
        public string MediaTypeCode { get; set; } = default!;
        public int RegionId { get; set; }
        public string? ExternalUrl { get; set; }
        public string? LegendText { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
