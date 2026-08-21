namespace YachayPeru.API.Contracts.Administration.Courses.Response
{
    public record CourseListResponse
    {
        public int Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public string? Description { get; init; }
        public bool IsActive { get; init; }
        public string? CoverImageUrl { get; init; }
        public DateTime CreatedAt { get; init; }
    }

    public record CourseDetailResponse
    {
        public int Id { get; init; }
        public string Title { get; init; } = string.Empty;
        public string? Description { get; init; }
        public bool IsActive { get; init; }
        public string? CoverImageUrl { get; init; }
        public int? SourceTemplateId { get; init; }
        public string? ZoneCode { get; init; }
        public string? AmbientAudioUrl { get; init; }
        public string? AmbientAudioTitle { get; init; }
        public string? SpotifyUrl { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}
