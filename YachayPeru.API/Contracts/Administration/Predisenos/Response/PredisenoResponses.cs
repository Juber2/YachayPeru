namespace YachayPeru.API.Contracts.Administration.Predisenos.Response
{
    public record PredisenoListItemResponse
    {
        public int Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public string TreeJson { get; init; } = string.Empty;
        public DateTime CreatedAt { get; init; }
    }

    public record PredisenoDetailResponse
    {
        public int Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public string TreeJson { get; init; } = string.Empty;
    }
}
