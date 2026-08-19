namespace YachayPeru.API.Contracts.Aprendiz.Noticias.Response
{
    public record AprendizNoticiaListItemResponse
    {
        public int Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public string Category { get; init; } = string.Empty;
        public string? ImageUrl { get; init; }
        public string Body { get; init; } = string.Empty;
        public DateTime CreatedAt { get; init; }
    }

    public record AprendizNoticiaDetailResponse
    {
        public int Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public string Category { get; init; } = string.Empty;
        public string Body { get; init; } = string.Empty;
        public string? ImageUrl { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}
