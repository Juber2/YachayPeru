namespace YachayPeru.API.Contracts.Aprendiz.Insignias.Response
{
    public record AprendizInsigniaListItemResponse
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string? ImageUrl { get; init; }
        public bool IsEarned { get; init; }
        public DateTime? EarnedAt { get; init; }
    }
}
