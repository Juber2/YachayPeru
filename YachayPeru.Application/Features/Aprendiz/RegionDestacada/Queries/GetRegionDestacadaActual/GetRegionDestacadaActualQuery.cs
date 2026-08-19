using MediatR;

namespace YachayPeru.Application.Features.Aprendiz.RegionDestacada.Queries.GetRegionDestacadaActual
{
    public record GetRegionDestacadaActualQuery : IRequest<AprendizRegionDestacadaItem?>;

    public class AprendizRegionDestacadaItem
    {
        public int RegionId { get; set; }
        public string RegionTitle { get; set; } = string.Empty;
        public string? RegionDescription { get; set; }
        public string? CoverImageUrl { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
