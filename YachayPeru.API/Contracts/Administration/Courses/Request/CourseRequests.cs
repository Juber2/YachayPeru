namespace YachayPeru.API.Contracts.Administration.Courses.Request
{
    public class CreateCourseRequest
    {
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public string? ZoneCode { get; set; }
    }

    public class EditCourseRequest
    {
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public string? ZoneCode { get; set; }
    }
}
