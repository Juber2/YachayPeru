namespace YachayPeru.API.Contracts.Aprendiz.Regiones.Response
{
    public record AprendizRegionListItemResponse
    {
        public int Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public string? Description { get; init; }
        public string? CoverImageUrl { get; init; }
        public int ProgressPercent { get; init; }
        public bool IsCompleted { get; init; }
    }

    public record AprendizRegionDetailResponse
    {
        public int Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public string? Description { get; init; }
        public string? CoverImageUrl { get; init; }
        public List<AprendizModuleResponse> Modules { get; init; } = [];
        public int RetoCount { get; init; }
        public int CompletedRetoCount { get; init; }
    }

    public record AprendizModuleResponse
    {
        public int Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public string? Description { get; init; }
        public int OrderIndex { get; init; }
        public decimal? DurationHours { get; init; }
        public List<AprendizModuleContentResponse> Contents { get; init; } = [];
    }

    public record AprendizModuleContentResponse
    {
        public int Id { get; init; }
        public string? Text { get; init; }
        public int OrderIndex { get; init; }
        public List<AprendizModuleContentFileResponse> Files { get; init; } = [];
    }

    public record AprendizModuleContentFileResponse
    {
        public int Id { get; init; }
        public string FileTypeCode { get; init; } = string.Empty;
        public string FileUrl { get; init; } = string.Empty;
        public string FileName { get; init; } = string.Empty;
        public int OrderIndex { get; init; }
    }
}
