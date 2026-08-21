using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YachayPeru.API.Authorization;
using YachayPeru.API.Contracts.Administration.Courses.Request;
using YachayPeru.API.Contracts.Administration.Courses.Response;
using YachayPeru.API.Contracts.Common;
using YachayPeru.API.Extensions;
using YachayPeru.Application.Features.Administration.Courses.Commands.CreateCourse;
using YachayPeru.Application.Features.Administration.Courses.Commands.DeleteCourse;
using YachayPeru.Application.Features.Administration.Courses.Commands.EditCourse;
using YachayPeru.Application.Features.Administration.Courses.Commands.UploadCoverImage;
using YachayPeru.Application.Features.Administration.Courses.Commands.UploadAmbientAudio;
using YachayPeru.Application.Features.Administration.Courses.Queries.GetCourseById;
using YachayPeru.Application.Features.Administration.Courses.Queries.GetCourses;

namespace YachayPeru.API.Controllers.Administration
{
    [ApiController]
    [Route("administration")]
    [Authorize]
    public class CoursesController : ControllerBase
    {
        private readonly IMediator mediator;

        public CoursesController(IMediator _mediator) => mediator = _mediator;

        [HttpGet("courses")]
        [Authorize(Policy = AppPermissions.CursosPlantilla.Read)]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var items = await mediator.Send(new GetCoursesQuery(), ct);
            var response = items.Select(c => new CourseListResponse
            {
                Id          = c.Id,
                Title       = c.Title,
                Description = c.Description,
                IsActive    = c.IsActive,
                CoverImageUrl = Request.ToAbsoluteUrl(c.CoverImageUrl),
                CreatedAt   = c.CreatedAt
            }).ToList();
            return Ok(ApiResponse<List<CourseListResponse>>.Ok(response));
        }

        [HttpGet("courses/{id:int}")]
        [Authorize(Policy = AppPermissions.CursosPlantilla.Read)]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var result = await mediator.Send(new GetCourseByIdQuery(id), ct);
            if (!result.IsSuccess) return this.FromResult(result);

            var detail = result.Value!;
            return Ok(ApiResponse<CourseDetailResponse>.Ok(new CourseDetailResponse
            {
                Id               = detail.Id,
                Title            = detail.Title,
                Description      = detail.Description,
                IsActive         = detail.IsActive,
                CoverImageUrl    = Request.ToAbsoluteUrl(detail.CoverImageUrl),
                SourceTemplateId = detail.SourceTemplateId,
                ZoneCode         = detail.ZoneCode,
                AmbientAudioUrl   = Request.ToAbsoluteUrl(detail.AmbientAudioUrl),
                AmbientAudioTitle = detail.AmbientAudioTitle,
                SpotifyUrl        = detail.SpotifyUrl,
                CreatedAt        = detail.CreatedAt
            }));
        }

        [HttpPost("courses")]
        [Authorize(Policy = AppPermissions.CursosPlantilla.Create)]
        public async Task<IActionResult> Create([FromBody] CreateCourseRequest request, CancellationToken ct)
        {
            var result = await mediator.Send(new CreateCourseCommand
            {
                Title       = request.Title,
                Description = request.Description,
                ZoneCode    = request.ZoneCode,
                AmbientAudioTitle = request.AmbientAudioTitle,
                SpotifyUrl        = request.SpotifyUrl
            }, ct);
            return this.FromResult(result);
        }

        [HttpPut("courses/{id:int}")]
        [Authorize(Policy = AppPermissions.CursosPlantilla.Update)]
        public async Task<IActionResult> Edit(int id, [FromBody] EditCourseRequest request, CancellationToken ct)
        {
            var result = await mediator.Send(new EditCourseCommand
            {
                Id          = id,
                Title       = request.Title,
                Description = request.Description,
                IsActive    = request.IsActive,
                ZoneCode    = request.ZoneCode,
                AmbientAudioTitle = request.AmbientAudioTitle,
                SpotifyUrl        = request.SpotifyUrl
            }, ct);
            return this.FromResult(result);
        }

        [HttpDelete("courses/{id:int}")]
        [Authorize(Policy = AppPermissions.CursosPlantilla.Delete)]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var result = await mediator.Send(new DeleteCourseCommand(id), ct);
            return this.FromResult(result);
        }

        [HttpPost("courses/{id:int}/cover-image")]
        [Authorize(Policy = AppPermissions.CursosPlantilla.Update)]
        public async Task<IActionResult> UploadCoverImage(int id, IFormFile file, CancellationToken ct)
        {
            if (file is null || file.Length == 0)
                return BadRequest(ApiResponse<string>.Fail("No se recibió ningún archivo."));

            await using var stream = file.OpenReadStream();
            var result = await mediator.Send(new UploadCoverImageCommand
            {
                Id = id,
                FileStream = stream,
                FileName = file.FileName
            }, ct);
            if (!result.IsSuccess) return this.FromResult(result);
            return Ok(ApiResponse<string>.Ok(Request.ToAbsoluteUrl(result.Value)));
        }

        [HttpPost("courses/{id:int}/ambient-audio")]
        [Authorize(Policy = AppPermissions.CursosPlantilla.Update)]
        public async Task<IActionResult> UploadAmbientAudio(int id, IFormFile file, CancellationToken ct)
        {
            if (file is null || file.Length == 0)
                return BadRequest(ApiResponse<string>.Fail("No se recibió ningún archivo."));

            await using var stream = file.OpenReadStream();
            var result = await mediator.Send(new UploadAmbientAudioCommand
            {
                Id = id,
                FileStream = stream,
                FileName = file.FileName
            }, ct);
            if (!result.IsSuccess) return this.FromResult(result);
            return Ok(ApiResponse<string>.Ok(Request.ToAbsoluteUrl(result.Value)));
        }
    }
}
