using MediatR;
using YachayPeru.Application.Abstractions.Persistence.Aprendiz;

namespace YachayPeru.Application.Features.Aprendiz.Comunidad.Queries.GetPosts
{
    public class GetPostsHandler : IRequestHandler<GetPostsQuery, IReadOnlyList<AprendizCommunityPostListItem>>
    {
        private readonly ICommunityPostRepository postRepository;
        private readonly ICommunityPostLikeRepository likeRepository;

        public GetPostsHandler(ICommunityPostRepository _postRepository, ICommunityPostLikeRepository _likeRepository)
        {
            postRepository = _postRepository;
            likeRepository = _likeRepository;
        }

        public async Task<IReadOnlyList<AprendizCommunityPostListItem>> Handle(GetPostsQuery request, CancellationToken ct)
        {
            var posts = (await postRepository.ListAsync(ct)).OrderByDescending(p => p.CreatedAt).ToList();
            var postIds = posts.Select(p => p.Id).ToList();

            var likeCounts = await likeRepository.CountByPostsAsync(postIds, ct);
            var likedPostIds = await likeRepository.GetLikedPostIdsAsync(request.UserId, postIds, ct);

            return posts.Select(p => new AprendizCommunityPostListItem
            {
                Id = p.Id,
                AuthorName = p.AuthorName,
                AuthorInitials = GetInitials(p.AuthorName),
                RegionId = p.RegionId,
                Text = p.Text,
                PhotoUrl = p.PhotoUrl,
                LikeCount = likeCounts.TryGetValue(p.Id, out var count) ? count : 0,
                LikedByMe = likedPostIds.Contains(p.Id),
                CreatedAt = p.CreatedAt
            }).ToList();
        }

        private static string GetInitials(string authorName)
        {
            var parts = authorName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var initials = string.Concat(parts.Take(2).Select(p => char.ToUpperInvariant(p[0])));
            return initials;
        }
    }
}
