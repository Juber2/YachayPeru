namespace YachayPeru.Application.Actions.Courses.Models
{
    public class AddModuleInput
    {
        public int CourseVersionId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal? DurationHours { get; set; }
    }

    public class EditModuleInput
    {
        public int ModuleId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal? DurationHours { get; set; }
    }

    public class ReorderModulesInput
    {
        public int CourseVersionId { get; set; }
        public List<ModuleOrderItem> Items { get; set; } = [];
    }

    public class ModuleOrderItem
    {
        public int ModuleId { get; set; }
        public int OrderIndex { get; set; }
    }
}
