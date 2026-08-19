using MediatR;

namespace YachayPeru.Application.Features.RegionDestacada.Queries.GetRegionesDestacadas
{
    public record GetRegionesDestacadasQuery : IRequest<IReadOnlyList<RegionDestacadaListItem>>;

    public class RegionDestacadaListItem
    {
        public int Id { get; set; }
        public int RegionId { get; set; }
        public string RegionTitle { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
