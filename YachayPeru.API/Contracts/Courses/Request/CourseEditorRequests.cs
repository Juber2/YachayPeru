namespace YachayPeru.API.Contracts.Courses.Request
{
    public class AddModuleRequest
    {
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public decimal? DurationHours { get; set; }
    }

    public class EditModuleRequest
    {
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public decimal? DurationHours { get; set; }
    }

    public class ReorderModulesRequest
    {
        public List<ModuleOrderEntryRequest> Items { get; set; } = [];
    }

    public class ModuleOrderEntryRequest
    {
        public int ModuleId { get; set; }
        public int OrderIndex { get; set; }
    }

    public class ReorderModuleContentsRequest
    {
        public List<ContentOrderEntryRequest> Items { get; set; } = [];
    }

    public class ContentOrderEntryRequest
    {
        public int ContentId { get; set; }
        public int OrderIndex { get; set; }
    }

    public class AddModuleContentRequest
    {
        public string? Text { get; set; }
        public string? DesignJson { get; set; }
    }

    public class EditModuleContentRequest
    {
        public string? Text { get; set; }
        public string? DesignJson { get; set; }
    }
}
