using MediatR;

namespace YachayPeru.Application.Features.Aprendiz.Comunidad.Queries.GetPosts
{
    public sealed record GetPostsQuery(int UserId) : IRequest<IReadOnlyList<AprendizCommunityPostListItem>>;

    public sealed record AprendizCommunityPostListItem
    {
        public int Id { get; init; }
        public string AuthorName { get; init; } = string.Empty;
        public string AuthorInitials { get; init; } = string.Empty;
        public int? RegionId { get; init; }
        public string Text { get; init; } = string.Empty;
        public string? PhotoUrl { get; init; }
        public int LikeCount { get; init; }
        public bool LikedByMe { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}
