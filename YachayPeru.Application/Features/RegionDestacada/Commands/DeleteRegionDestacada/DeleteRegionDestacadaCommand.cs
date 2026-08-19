using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.RegionDestacada.Commands.DeleteRegionDestacada
{
    public record DeleteRegionDestacadaCommand(int Id) : IRequest<Result>;
}
