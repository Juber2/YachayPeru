namespace YachayPeru.Application.Features.Administration.Courses.Queries.GetCourses
{
    public class CourseListItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public string? CoverImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
