using MediatR;

namespace YachayPeru.Application.Features.Calendario.Queries.GetFestividades
{
    public record GetFestividadesQuery : IRequest<IReadOnlyList<FestividadListItem>>;

    public class FestividadListItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int RegionId { get; set; }
        public string RegionTitle { get; set; } = string.Empty;
        public int Month { get; set; }
        public int Day { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
