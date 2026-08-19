namespace YachayPeru.API.Contracts.Administration.Noticias.Response
{
    public record NoticiaListItemResponse
    {
        public int Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public string Category { get; init; } = string.Empty;
        public string? ImageUrl { get; init; }
        public bool IsActive { get; init; }
        public DateTime CreatedAt { get; init; }
    }

    public record NoticiaDetailResponse
    {
        public int Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public string Category { get; init; } = string.Empty;
        public string Body { get; init; } = string.Empty;
        public string? ImageUrl { get; init; }
        public bool IsActive { get; init; }
    }
}
