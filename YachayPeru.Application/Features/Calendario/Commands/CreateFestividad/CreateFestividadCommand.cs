using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Calendario.Commands.CreateFestividad
{
    public sealed record CreateFestividadCommand : IRequest<Result<int>>
    {
        public string Name { get; init; } = default!;
        public string? Description { get; init; }
        public int RegionId { get; init; }
        public int Month { get; init; }
        public int Day { get; init; }
        public bool IsActive { get; init; } = true;
    }
}
