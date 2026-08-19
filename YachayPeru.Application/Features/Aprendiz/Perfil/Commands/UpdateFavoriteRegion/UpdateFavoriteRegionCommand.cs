using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Aprendiz.Perfil.Commands.UpdateFavoriteRegion
{
    public sealed record UpdateFavoriteRegionCommand : IRequest<Result>
    {
        public int UserId { get; init; }
        public int RegionId { get; init; }
    }
}
