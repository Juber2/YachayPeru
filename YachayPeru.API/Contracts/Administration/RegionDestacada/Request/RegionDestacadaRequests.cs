namespace YachayPeru.API.Contracts.Administration.RegionDestacada.Request
{
    public class UpsertRegionDestacadaRequest
    {
        public int RegionId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
