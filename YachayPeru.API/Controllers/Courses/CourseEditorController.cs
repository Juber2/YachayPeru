using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YachayPeru.API.Authorization;
using YachayPeru.API.Contracts.Common;
using YachayPeru.API.Contracts.Courses.Request;
using YachayPeru.API.Contracts.Courses.Response;
using YachayPeru.API.Extensions;
using YachayPeru.Application.Features.Courses.Commands.Content;
using YachayPeru.Application.Features.Courses.Queries.GetCourseContent;
using YachayPeru.Domain.Constants;

namespace YachayPeru.API.Controllers.Courses
{
    [ApiController]
    [Route("courses/{courseId:int}")]
    [Authorize]
    public class CourseEditorController : ControllerBase
    {
        private readonly IMediator mediator;

        public CourseEditorController(IMediator _mediator) => mediator = _mediator;

        // ── Contenido ────────────────────────────────────────────────────────────

        [HttpGet("content")]
        [Authorize(Policy = AppPermissions.CourseEditor.Read)]
        public async Task<IActionResult> GetContent(int courseId, CancellationToken ct)
        {
            var result = await mediator.Send(new GetCourseContentQuery(courseId), ct);
            if (!result.IsSuccess) return this.FromResult(result);

            var d = result.Value!;
            return Ok(ApiResponse<CourseContentResponse>.Ok(new CourseContentResponse
            {
                CourseVersionId = d.CourseVersionId,
                VersionNumber   = d.VersionNumber,
                StatusCode      = d.StatusCode,
                DurationHours   = d.DurationHours,
                Modules         = d.Modules.Select(m => new ModuleResponse
                {
                    Id            = m.Id,
                    Title         = m.Title,
                    Description   = m.Description,
                    OrderIndex    = m.OrderIndex,
                    DurationHours = m.DurationHours,
                    Contents      = m.Contents.Select(c => new ContentResponse
                    {
                        Id                    = c.Id,
                        Text                  = c.Text,
                        DesignJson            = c.DesignJson,
                        OrderIndex            = c.OrderIndex,
                        Files                 = c.Files.Select(f => new FileResponse
                        {
                            Id           = f.Id,
                            FileTypeCode = f.FileTypeCode,
                            FileUrl      = Request.ToAbsoluteUrl(f.FileUrl)!,
                            FileName     = f.FileName,
                            OrderIndex   = f.OrderIndex
                        }).ToList()
                    }).ToList()
                }).ToList()
            }));
        }

        [HttpPost("content/versions/{versionId:int}/course_modules")]
        [Authorize(Policy = AppPermissions.CourseEditor.Update)]
        public async Task<IActionResult> AddModule(int versionId, [FromBody] AddModuleRequest request, CancellationToken ct)
        {
            var result = await mediator.Send(new AddModuleCommand
            {
                CourseVersionId = versionId,
                Title           = request.Title,
                Description     = request.Description,
                DurationHours   = request.DurationHours
            }, ct);
            return this.FromResult(result);
        }

        [HttpPut("content/course_modules/{moduleId:int}")]
        [Authorize(Policy = AppPermissions.CourseEditor.Update)]
        public async Task<IActionResult> EditModule(int moduleId, [FromBody] EditModuleRequest request, CancellationToken ct)
        {
            var result = await mediator.Send(new EditModuleCommand
            {
                ModuleId      = moduleId,
                Title         = request.Title,
                Description   = request.Description,
                DurationHours = request.DurationHours
            }, ct);
            return this.FromResult(result);
        }

        [HttpDelete("content/course_modules/{moduleId:int}")]
        [Authorize(Policy = AppPermissions.CourseEditor.Update)]
        public async Task<IActionResult> DeleteModule(int moduleId, CancellationToken ct)
        {
            var result = await mediator.Send(new DeleteModuleCommand(moduleId), ct);
            return this.FromResult(result);
        }

        [HttpPut("content/versions/{versionId:int}/course_modules/reorder")]
        [Authorize(Policy = AppPermissions.CourseEditor.Update)]
        public async Task<IActionResult> ReorderModules(int versionId, [FromBody] ReorderModulesRequest request, CancellationToken ct)
        {
            var result = await mediator.Send(new ReorderModulesCommand
            {
                CourseVersionId = versionId,
                Items           = request.Items.Select(i => new ModuleOrderEntry { ModuleId = i.ModuleId, OrderIndex = i.OrderIndex }).ToList()
            }, ct);
            return this.FromResult(result);
        }

        [HttpPut("content/course_modules/{moduleId:int}/module_contents/reorder")]
        [Authorize(Policy = AppPermissions.CourseEditor.Update)]
        public async Task<IActionResult> ReorderContents(int moduleId, [FromBody] ReorderModuleContentsRequest request, CancellationToken ct)
        {
            var result = await mediator.Send(new ReorderModuleContentsCommand
            {
                ModuleId = moduleId,
                Items    = request.Items.Select(i => new ContentOrderEntry { ContentId = i.ContentId, OrderIndex = i.OrderIndex }).ToList()
            }, ct);
            return this.FromResult(result);
        }

        [HttpPost("content/course_modules/{moduleId:int}/module_contents")]
        [Authorize(Policy = AppPermissions.CourseEditor.Update)]
        public async Task<IActionResult> AddContent(int moduleId, [FromBody] AddModuleContentRequest request, CancellationToken ct)
        {
            var result = await mediator.Send(new AddModuleContentCommand
            {
                ModuleId              = moduleId,
                Text                  = request.Text,
                DesignJson            = request.DesignJson
            }, ct);
            return this.FromResult(result);
        }

        [HttpPut("content/module_contents/{contentId:int}")]
        [Authorize(Policy = AppPermissions.CourseEditor.Update)]
        public async Task<IActionResult> EditContent(int contentId, [FromBody] EditModuleContentRequest request, CancellationToken ct)
        {
            var result = await mediator.Send(new EditModuleContentCommand
            {
                ContentId  = contentId,
                Text       = request.Text,
                DesignJson = request.DesignJson
            }, ct);
            return this.FromResult(result);
        }

        [HttpDelete("content/module_contents/{contentId:int}")]
        [Authorize(Policy = AppPermissions.CourseEditor.Update)]
        public async Task<IActionResult> DeleteContent(int contentId, CancellationToken ct)
        {
            var result = await mediator.Send(new DeleteModuleContentCommand(contentId), ct);
            return this.FromResult(result);
        }

        [HttpPost("content/module_contents/{contentId:int}/files")]
        [Authorize(Policy = AppPermissions.CourseEditor.Update)]
        public async Task<IActionResult> UploadContentFile(int contentId, IFormFile file, CancellationToken ct)
        {
            if (file is null || file.Length == 0)
                return BadRequest(ApiResponse<string>.Fail("No se recibió ningún archivo."));

            var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
            var fileTypeCode = ext switch
            {
                ".jpg" or ".jpeg" or ".png" or ".gif" or ".webp" => AppConstants.FileTypeCode.Image,
                ".mp4" or ".mov" or ".avi" or ".mkv"             => AppConstants.FileTypeCode.Video,
                _                                                 => AppConstants.FileTypeCode.Document
            };

            await using var stream = file.OpenReadStream();
            var result = await mediator.Send(new UploadItemFileCommand
            {
                ItemId       = contentId,
                FileTypeCode = fileTypeCode,
                FileStream   = stream,
                FileName     = file.FileName
            }, ct);
            if (!result.IsSuccess) return this.FromResult(result);
            return Ok(ApiResponse<string>.Ok(Request.ToAbsoluteUrl(result.Value)));
        }

        [HttpGet("content/versions")]
        [Authorize(Policy = AppPermissions.CourseEditor.Read)]
        public async Task<IActionResult> GetVersions(int courseId, CancellationToken ct)
        {
            var items = await mediator.Send(new GetCourseVersionsQuery(courseId), ct);
            var response = items.Select(v => new VersionSummaryResponse
            {
                Id            = v.Id,
                VersionNumber = v.VersionNumber,
                StatusCode    = v.StatusCode,
                IsCurrent     = v.IsCurrent,
                PublishedAt   = v.PublishedAt,
                CreatedAt     = v.CreatedAt
            }).ToList();
            return Ok(ApiResponse<List<VersionSummaryResponse>>.Ok(response));
        }

        [HttpGet("content/versions/{versionId:int}")]
        [Authorize(Policy = AppPermissions.CourseEditor.Read)]
        public async Task<IActionResult> GetVersionDetail(int courseId, int versionId, CancellationToken ct)
        {
            var result = await mediator.Send(new GetCourseVersionDetailQuery(versionId, courseId), ct);
            if (!result.IsSuccess) return this.FromResult(result);

            var d = result.Value!;
            return Ok(ApiResponse<VersionDetailResponse>.Ok(new VersionDetailResponse
            {
                Id            = d.Id,
                VersionNumber = d.VersionNumber,
                StatusCode    = d.StatusCode,
                DurationHours = d.DurationHours,
                IsCurrent     = d.IsCurrent,
                PublishedAt   = d.PublishedAt,
                CreatedAt     = d.CreatedAt,
                Modules       = d.Modules.Select(m => new ModuleResponse
                {
                    Id            = m.Id,
                    Title         = m.Title,
                    Description   = m.Description,
                    OrderIndex    = m.OrderIndex,
                    DurationHours = m.DurationHours,
                    Contents      = m.Contents.Select(c => new ContentResponse
                    {
                        Id                    = c.Id,
                        Text                  = c.Text,
                        DesignJson            = c.DesignJson,
                        OrderIndex            = c.OrderIndex,
                        Files                 = c.Files.Select(f => new FileResponse
                        {
                            Id           = f.Id,
                            FileTypeCode = f.FileTypeCode,
                            FileUrl      = Request.ToAbsoluteUrl(f.FileUrl)!,
                            FileName     = f.FileName,
                            OrderIndex   = f.OrderIndex
                        }).ToList()
                    }).ToList()
                }).ToList()
            }));
        }

        [HttpPost("content/draft")]
        [Authorize(Policy = AppPermissions.CourseEditor.Update)]
        public async Task<IActionResult> CreateDraft(int courseId, CancellationToken ct)
        {
            var result = await mediator.Send(new CreateContentDraftCommand(courseId), ct);
            return this.FromResult(result);
        }

        [HttpDelete("content/draft")]
        [Authorize(Policy = AppPermissions.CourseEditor.Update)]
        public async Task<IActionResult> DiscardDraft(int courseId, CancellationToken ct)
        {
            var result = await mediator.Send(new DiscardContentDraftCommand(courseId), ct);
            return this.FromResult(result);
        }

        [HttpPost("content/publish")]
        [Authorize(Policy = AppPermissions.CourseEditor.Update)]
        public async Task<IActionResult> PublishContent(int courseId, CancellationToken ct)
        {
            var result = await mediator.Send(new PublishContentVersionCommand(courseId), ct);
            return this.FromResult(result);
        }

        [HttpPost("content/versions/{versionId:int}/restore")]
        [Authorize(Policy = AppPermissions.CourseEditor.Update)]
        public async Task<IActionResult> RestoreVersion(int courseId, int versionId, CancellationToken ct)
        {
            var result = await mediator.Send(new RestoreContentVersionCommand { VersionId = versionId, CourseId = courseId }, ct);
            return this.FromResult(result);
        }
    }
}
