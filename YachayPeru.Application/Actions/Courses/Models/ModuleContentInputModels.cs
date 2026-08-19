namespace YachayPeru.Application.Actions.Courses.Models
{
    public class AddModuleContentInput
    {
        public int ModuleId { get; set; }
        public string? Text { get; set; }
        public string? DesignJson { get; set; }
        public List<ContentFileInput> Files { get; set; } = [];
    }

    public class EditModuleContentInput
    {
        public int ContentId { get; set; }
        public string? Text { get; set; }
        public string? DesignJson { get; set; }
        public List<ContentFileInput> Files { get; set; } = [];
    }

    public class ContentFileInput
    {
        public string FileTypeCode { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;
        public int OrderIndex { get; set; }
    }

    public class ReorderContentsInput
    {
        public int ModuleId { get; set; }
        public List<ContentOrderItem> Items { get; set; } = [];
    }

    public class ContentOrderItem
    {
        public int ContentId { get; set; }
        public int OrderIndex { get; set; }
    }
}
