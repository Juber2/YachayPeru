using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.RegionDestacada.Commands.EditRegionDestacada
{
    public sealed record EditRegionDestacadaCommand : IRequest<Result>
    {
        public int Id { get; init; }
        public int RegionId { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
    }
}
