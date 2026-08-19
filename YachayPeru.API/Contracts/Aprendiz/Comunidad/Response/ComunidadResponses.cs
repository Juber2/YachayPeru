namespace YachayPeru.API.Contracts.Aprendiz.Comunidad.Response
{
    public record AprendizCommunityPostResponse
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

    public record AprendizPostLikeResponse
    {
        public bool LikedByMe { get; init; }
        public int LikeCount { get; init; }
    }
}
