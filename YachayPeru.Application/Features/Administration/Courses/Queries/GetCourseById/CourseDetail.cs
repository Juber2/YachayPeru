namespace YachayPeru.Application.Features.Administration.Courses.Queries.GetCourseById
{
    public class CourseDetail
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public string? CoverImageUrl { get; set; }
        public int? SourceTemplateId { get; set; }
        public string? ZoneCode { get; set; }
        public string? AmbientAudioUrl { get; set; }
        public string? AmbientAudioTitle { get; set; }
        public string? SpotifyUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
