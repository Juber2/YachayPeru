using MediatR;
using YachayPeru.Application.Abstractions.Persistence;
using YachayPeru.Application.Abstractions.Persistence.Aprendiz;
using YachayPeru.Application.Common.Results;
using YachayPeru.Domain.Entities.Aprendiz;
using static YachayPeru.Application.Common.Results.ResultCodes;

namespace YachayPeru.Application.Features.Aprendiz.Comunidad.Commands.PostLike
{
    public class PostLikeHandler : IRequestHandler<PostLikeCommand, Result<PostLikeResult>>
    {
        private readonly ICommunityPostRepository postRepository;
        private readonly ICommunityPostLikeRepository likeRepository;
        private readonly IUnitOfWork unitOfWork;

        public PostLikeHandler(
            ICommunityPostRepository _postRepository,
            ICommunityPostLikeRepository _likeRepository,
            IUnitOfWork _unitOfWork)
        {
            postRepository = _postRepository;
            likeRepository = _likeRepository;
            unitOfWork = _unitOfWork;
        }

        public async Task<Result<PostLikeResult>> Handle(PostLikeCommand request, CancellationToken ct)
        {
            var post = await postRepository.GetByIdAsync(request.PostId, ct);
            if (post is null)
                return Result<PostLikeResult>.Failure("Publicación no encontrada.", NotFound);

            var existingLike = await likeRepository.GetByPostAndUserAsync(request.PostId, request.UserId, ct);
            bool likedByMe;

            if (existingLike is null)
            {
                await likeRepository.AddAsync(new CommunityPostLike
                {
                    PostId = request.PostId,
                    UserId = request.UserId,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = request.UserId
                }, ct);
                likedByMe = true;
            }
            else
            {
                likeRepository.Delete(existingLike);
                likedByMe = false;
            }

            await unitOfWork.SaveChangesAsync(ct);
            var likeCount = await likeRepository.CountByPostAsync(request.PostId, ct);

            return Result<PostLikeResult>.Success(new PostLikeResult
            {
                LikedByMe = likedByMe,
                LikeCount = likeCount
            });
        }
    }
}
