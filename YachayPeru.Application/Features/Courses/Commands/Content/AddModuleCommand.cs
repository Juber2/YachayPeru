using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Courses.Commands.Content
{
    public sealed record AddModuleCommand : IRequest<Result<int>>
    {
        public int CourseVersionId { get; init; }
        public string Title { get; init; } = default!;
        public string? Description { get; init; }
        public decimal? DurationHours { get; init; }
    }

    public sealed record EditModuleCommand : IRequest<Result<int>>
    {
        public int ModuleId { get; init; }
        public string Title { get; init; } = default!;
        public string? Description { get; init; }
        public decimal? DurationHours { get; init; }
    }

    public record DeleteModuleCommand(int ModuleId) : IRequest<Result>;

    public sealed record ReorderModulesCommand : IRequest<Result>
    {
        public int CourseVersionId { get; init; }
        public List<ModuleOrderEntry> Items { get; init; } = [];
    }

    public class ModuleOrderEntry
    {
        public int ModuleId { get; set; }
        public int OrderIndex { get; set; }
    }

    public sealed record AddModuleContentCommand : IRequest<Result<int>>
    {
        public int ModuleId { get; init; }
        public string? Text { get; init; }
        public string? DesignJson { get; init; }
    }

    public sealed record EditModuleContentCommand : IRequest<Result<int>>
    {
        public int ContentId { get; init; }
        public string? Text { get; init; }
        public string? DesignJson { get; init; }
    }

    public record DeleteModuleContentCommand(int ContentId) : IRequest<Result>;

    public sealed record ReorderModuleContentsCommand : IRequest<Result>
    {
        public int ModuleId { get; init; }
        public List<ContentOrderEntry> Items { get; init; } = [];
    }

    public class ContentOrderEntry
    {
        public int ContentId { get; set; }
        public int OrderIndex { get; set; }
    }

    public record CreateContentDraftCommand(int CourseId) : IRequest<Result<int>>;
    public record DiscardContentDraftCommand(int CourseId) : IRequest<Result>;

    public record PublishContentVersionCommand(int CourseId) : IRequest<Result<int>>;

    public sealed record RestoreContentVersionCommand : IRequest<Result<int>>
    {
        public int VersionId { get; init; }
        public int CourseId { get; init; }
    }
}
