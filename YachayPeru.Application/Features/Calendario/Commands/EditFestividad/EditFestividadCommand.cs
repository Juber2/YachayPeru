using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Calendario.Commands.EditFestividad
{
    public sealed record EditFestividadCommand : IRequest<Result>
    {
        public int Id { get; init; }
        public string Name { get; init; } = default!;
        public string? Description { get; init; }
        public int RegionId { get; init; }
        public int Month { get; init; }
        public int Day { get; init; }
        public bool IsActive { get; init; }
    }
}
