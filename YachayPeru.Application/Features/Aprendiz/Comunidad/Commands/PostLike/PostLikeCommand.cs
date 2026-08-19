using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Aprendiz.Comunidad.Commands.PostLike
{
    public sealed record PostLikeCommand(int UserId, int PostId) : IRequest<Result<PostLikeResult>>;

    public sealed record PostLikeResult
    {
        public bool LikedByMe { get; init; }
        public int LikeCount { get; init; }
    }
}
