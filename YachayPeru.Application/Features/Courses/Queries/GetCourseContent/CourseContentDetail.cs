using YachayPeru.Application.Features.Courses.Queries.GetCourseContent;

namespace YachayPeru.Application.Actions.Courses
{
    public class VersionSummary
    {
        public int Id { get; set; }
        public int VersionNumber { get; set; }
        public string StatusCode { get; set; } = string.Empty;
        public bool IsCurrent { get; set; }
        public DateTime? PublishedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class VersionDetail
    {
        public int Id { get; set; }
        public int VersionNumber { get; set; }
        public string StatusCode { get; set; } = string.Empty;
        public decimal? DurationHours { get; set; }
        public bool IsCurrent { get; set; }
        public DateTime? PublishedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<ModuleDto> Modules { get; set; } = [];
    }
}

namespace YachayPeru.Application.Features.Courses.Queries.GetCourseContent
{
    public class CourseContentDetail
    {
        public int CourseVersionId { get; set; }
        public int VersionNumber { get; set; }
        public string StatusCode { get; set; } = string.Empty;
        public decimal? DurationHours { get; set; }
        public List<ModuleDto> Modules { get; set; } = [];
    }

    public class ModuleDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int OrderIndex { get; set; }
        public decimal? DurationHours { get; set; }
        public List<ContentDto> Contents { get; set; } = [];
    }

    public class ContentDto
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public string? DesignJson { get; set; }
        public int OrderIndex { get; set; }
        public List<FileDto> Files { get; set; } = [];
    }

    public class FileDto
    {
        public int Id { get; set; }
        public string FileTypeCode { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public int OrderIndex { get; set; }
    }
}
