using MediatR;

namespace YachayPeru.Application.Features.Aprendiz.Calendario.Queries.GetProximaFestividad
{
    public sealed record GetProximaFestividadQuery(int UserId) : IRequest<AprendizProximaFestividadItem?>;

    public sealed record AprendizProximaFestividadItem
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
