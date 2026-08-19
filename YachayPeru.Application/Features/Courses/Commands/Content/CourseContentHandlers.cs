using MediatR;
using YachayPeru.Application.Actions.Courses;
using YachayPeru.Application.Actions.Courses.Models;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Courses.Commands.Content
{
    public class AddModuleHandler : IRequestHandler<AddModuleCommand, Result<int>>
    {
        private readonly CourseContentActions contentActions;
        public AddModuleHandler(CourseContentActions _contentActions) => contentActions = _contentActions;
        public Task<Result<int>> Handle(AddModuleCommand request, CancellationToken ct)
            => contentActions.AddModule(new AddModuleInput
            {
                CourseVersionId = request.CourseVersionId,
                Title = request.Title,
                Description = request.Description,
                DurationHours = request.DurationHours
            }, ct);
    }

    public class EditModuleHandler : IRequestHandler<EditModuleCommand, Result<int>>
    {
        private readonly CourseContentActions contentActions;
        public EditModuleHandler(CourseContentActions _contentActions) => contentActions = _contentActions;
        public Task<Result<int>> Handle(EditModuleCommand request, CancellationToken ct)
            => contentActions.EditModule(new EditModuleInput
            {
                ModuleId = request.ModuleId,
                Title = request.Title,
                Description = request.Description,
                DurationHours = request.DurationHours
            }, ct);
    }

    public class DeleteModuleHandler : IRequestHandler<DeleteModuleCommand, Result>
    {
        private readonly CourseContentActions contentActions;
        public DeleteModuleHandler(CourseContentActions _contentActions) => contentActions = _contentActions;
        public Task<Result> Handle(DeleteModuleCommand request, CancellationToken ct)
            => contentActions.DeleteModule(request.ModuleId, ct);
    }

    public class ReorderModulesHandler : IRequestHandler<ReorderModulesCommand, Result>
    {
        private readonly CourseContentActions contentActions;
        public ReorderModulesHandler(CourseContentActions _contentActions) => contentActions = _contentActions;
        public Task<Result> Handle(ReorderModulesCommand request, CancellationToken ct)
            => contentActions.ReorderModules(new ReorderModulesInput
            {
                CourseVersionId = request.CourseVersionId,
                Items = request.Items.Select(i => new ModuleOrderItem { ModuleId = i.ModuleId, OrderIndex = i.OrderIndex }).ToList()
            }, ct);
    }

    public class AddModuleContentHandler : IRequestHandler<AddModuleContentCommand, Result<int>>
    {
        private readonly CourseContentActions contentActions;
        public AddModuleContentHandler(CourseContentActions _contentActions) => contentActions = _contentActions;
        public Task<Result<int>> Handle(AddModuleContentCommand request, CancellationToken ct)
            => contentActions.AddContent(new AddModuleContentInput
            {
                ModuleId = request.ModuleId,
                Text = request.Text,
                DesignJson = request.DesignJson
            }, ct);
    }

    public class EditModuleContentHandler : IRequestHandler<EditModuleContentCommand, Result<int>>
    {
        private readonly CourseContentActions contentActions;
        public EditModuleContentHandler(CourseContentActions _contentActions) => contentActions = _contentActions;
        public Task<Result<int>> Handle(EditModuleContentCommand request, CancellationToken ct)
            => contentActions.EditContent(new EditModuleContentInput
            {
                ContentId = request.ContentId,
                Text = request.Text,
                DesignJson = request.DesignJson
            }, ct);
    }

    public class DeleteModuleContentHandler : IRequestHandler<DeleteModuleContentCommand, Result>
    {
        private readonly CourseContentActions contentActions;
        public DeleteModuleContentHandler(CourseContentActions _contentActions) => contentActions = _contentActions;
        public Task<Result> Handle(DeleteModuleContentCommand request, CancellationToken ct)
            => contentActions.DeleteContent(request.ContentId, ct);
    }

    public class ReorderModuleContentsHandler : IRequestHandler<ReorderModuleContentsCommand, Result>
    {
        private readonly CourseContentActions contentActions;
        public ReorderModuleContentsHandler(CourseContentActions _contentActions) => contentActions = _contentActions;
        public Task<Result> Handle(ReorderModuleContentsCommand request, CancellationToken ct)
            => contentActions.ReorderContents(new ReorderContentsInput
            {
                ModuleId = request.ModuleId,
                Items    = request.Items.Select(i => new ContentOrderItem { ContentId = i.ContentId, OrderIndex = i.OrderIndex }).ToList()
            }, ct);
    }

    public class CreateContentDraftHandler : IRequestHandler<CreateContentDraftCommand, Result<int>>
    {
        private readonly CourseContentActions contentActions;
        public CreateContentDraftHandler(CourseContentActions _contentActions) => contentActions = _contentActions;
        public Task<Result<int>> Handle(CreateContentDraftCommand request, CancellationToken ct)
            => contentActions.CreateDraft(request.CourseId, ct);
    }

    public class DiscardContentDraftHandler : IRequestHandler<DiscardContentDraftCommand, Result>
    {
        private readonly CourseContentActions contentActions;
        public DiscardContentDraftHandler(CourseContentActions _contentActions) => contentActions = _contentActions;
        public Task<Result> Handle(DiscardContentDraftCommand request, CancellationToken ct)
            => contentActions.DiscardDraft(request.CourseId, ct);
    }

    public class PublishContentVersionHandler : IRequestHandler<PublishContentVersionCommand, Result<int>>
    {
        private readonly CourseContentActions contentActions;
        public PublishContentVersionHandler(CourseContentActions _contentActions) => contentActions = _contentActions;
        public Task<Result<int>> Handle(PublishContentVersionCommand request, CancellationToken ct)
            => contentActions.PublishContentVersion(request.CourseId, ct);
    }

    public class RestoreContentVersionHandler : IRequestHandler<RestoreContentVersionCommand, Result<int>>
    {
        private readonly CourseContentActions contentActions;
        public RestoreContentVersionHandler(CourseContentActions _contentActions) => contentActions = _contentActions;
        public Task<Result<int>> Handle(RestoreContentVersionCommand request, CancellationToken ct)
            => contentActions.RestoreContentVersion(request.VersionId, request.CourseId, ct);
    }
}
