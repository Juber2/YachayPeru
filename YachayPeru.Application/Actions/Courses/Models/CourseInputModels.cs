namespace YachayPeru.Application.Actions.Courses.Models
{
    public class CreateCourseInput
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ZoneCode { get; set; }
        public string? AmbientAudioTitle { get; set; }
        public string? SpotifyUrl { get; set; }
    }

    public class UpdateCourseInfoInput
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public string? ZoneCode { get; set; }
        public string? AmbientAudioTitle { get; set; }
        public string? SpotifyUrl { get; set; }
    }
}
