using MediatR;

namespace YachayPeru.Application.Features.Aprendiz.Calendario.Queries.GetFestividades
{
    public sealed record GetFestividadesQuery(int UserId) : IRequest<IReadOnlyList<AprendizFestividadListItem>>;

    public sealed record AprendizFestividadListItem
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
}
