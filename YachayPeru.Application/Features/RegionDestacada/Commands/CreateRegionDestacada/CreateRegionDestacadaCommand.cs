using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.RegionDestacada.Commands.CreateRegionDestacada
{
    public sealed record CreateRegionDestacadaCommand : IRequest<Result<int>>
    {
        public int RegionId { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
    }
}
