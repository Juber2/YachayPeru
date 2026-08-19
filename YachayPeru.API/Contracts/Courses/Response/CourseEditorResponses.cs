namespace YachayPeru.API.Contracts.Courses.Response
{
    public record CourseContentResponse
    {
        public int CourseVersionId { get; init; }
        public int VersionNumber { get; init; }
        public string StatusCode { get; init; } = string.Empty;
        public decimal? DurationHours { get; init; }
        public List<ModuleResponse> Modules { get; init; } = [];
    }

    public record ModuleResponse
    {
        public int Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public string? Description { get; init; }
        public int OrderIndex { get; init; }
        public decimal? DurationHours { get; init; }
        public List<ContentResponse> Contents { get; init; } = [];
    }

    public record ContentResponse
    {
        public int Id { get; init; }
        public string? Text { get; init; }
        public string? DesignJson { get; init; }
        public int OrderIndex { get; init; }
        public List<FileResponse> Files { get; init; } = [];
    }

    public record FileResponse
    {
        public int Id { get; init; }
        public string FileTypeCode { get; init; } = string.Empty;
        public string FileUrl { get; init; } = string.Empty;
        public string FileName { get; init; } = string.Empty;
        public int OrderIndex { get; init; }
    }

    public record VersionSummaryResponse
    {
        public int Id { get; init; }
        public int VersionNumber { get; init; }
        public string StatusCode { get; init; } = string.Empty;
        public bool IsCurrent { get; init; }
        public DateTime? PublishedAt { get; init; }
        public DateTime CreatedAt { get; init; }
    }

    public record VersionDetailResponse
    {
        public int Id { get; init; }
        public int VersionNumber { get; init; }
        public string StatusCode { get; init; } = string.Empty;
        public decimal? DurationHours { get; init; }
        public bool IsCurrent { get; init; }
        public DateTime? PublishedAt { get; init; }
        public DateTime CreatedAt { get; init; }
        public List<ModuleResponse> Modules { get; init; } = [];
    }
}
