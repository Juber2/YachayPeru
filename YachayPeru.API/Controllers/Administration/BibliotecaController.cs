using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YachayPeru.API.Authorization;
using YachayPeru.API.Contracts.Common;
using YachayPeru.API.Contracts.Administration.Biblioteca.Request;
using YachayPeru.API.Contracts.Administration.Biblioteca.Response;
using YachayPeru.API.Extensions;
using YachayPeru.Application.Features.Biblioteca.Commands.CreateMediaItem;
using YachayPeru.Application.Features.Biblioteca.Commands.DeleteMediaItem;
using YachayPeru.Application.Features.Biblioteca.Commands.EditMediaItem;
using YachayPeru.Application.Features.Biblioteca.Commands.UploadMediaItemThumbnail;
using YachayPeru.Application.Features.Biblioteca.Queries.GetMediaItemById;
using YachayPeru.Application.Features.Biblioteca.Queries.GetMediaItems;

namespace YachayPeru.API.Controllers.Administration
{
    [ApiController]
    [Route("administration/biblioteca")]
    [Authorize]
    public class BibliotecaController : ControllerBase
    {
        private readonly IMediator mediator;

        public BibliotecaController(IMediator _mediator) => mediator = _mediator;

        [HttpGet]
        [Authorize(Policy = AppPermissions.Biblioteca.Read)]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var items = await mediator.Send(new GetMediaItemsQuery(), ct);
            var response = items.Select(m => new MediaItemListItemResponse
            {
                Id = m.Id,
                Title = m.Title,
                MediaTypeCode = m.MediaTypeCode,
                RegionId = m.RegionId,
                RegionTitle = m.RegionTitle,
                ThumbnailUrl = Request.ToAbsoluteUrl(m.ThumbnailUrl),
                IsActive = m.IsActive,
                CreatedAt = m.CreatedAt
            }).ToList();
            return Ok(ApiResponse<List<MediaItemListItemResponse>>.Ok(response));
        }

        [HttpGet("{id:int}")]
        [Authorize(Policy = AppPermissions.Biblioteca.Read)]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var result = await mediator.Send(new GetMediaItemByIdQuery(id), ct);
            if (!result.IsSuccess) return this.FromResult(result);

            var d = result.Value!;
            return Ok(ApiResponse<MediaItemDetailResponse>.Ok(new MediaItemDetailResponse
            {
                Id = d.Id,
                Title = d.Title,
                MediaTypeCode = d.MediaTypeCode,
                RegionId = d.RegionId,
                RegionTitle = d.RegionTitle,
                ThumbnailUrl = Request.ToAbsoluteUrl(d.ThumbnailUrl),
                ExternalUrl = d.ExternalUrl,
                LegendText = d.LegendText,
                IsActive = d.IsActive
            }));
        }

        [HttpPost]
        [Authorize(Policy = AppPermissions.Biblioteca.Create)]
        public async Task<IActionResult> Create([FromBody] UpsertMediaItemRequest request, CancellationToken ct)
        {
            var result = await mediator.Send(new CreateMediaItemCommand
            {
                Title = request.Title,
                MediaTypeCode = request.MediaTypeCode,
                RegionId = request.RegionId,
                ExternalUrl = request.ExternalUrl,
                LegendText = request.LegendText,
                IsActive = request.IsActive
            }, ct);
            return this.FromResult(result);
        }

        [HttpPut("{id:int}")]
        [Authorize(Policy = AppPermissions.Biblioteca.Update)]
        public async Task<IActionResult> Edit(int id, [FromBody] UpsertMediaItemRequest request, CancellationToken ct)
        {
            var result = await mediator.Send(new EditMediaItemCommand
            {
                Id = id,
                Title = request.Title,
                MediaTypeCode = request.MediaTypeCode,
                RegionId = request.RegionId,
                ExternalUrl = request.ExternalUrl,
                LegendText = request.LegendText,
                IsActive = request.IsActive
            }, ct);
            return this.FromResult(result);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = AppPermissions.Biblioteca.Delete)]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var result = await mediator.Send(new DeleteMediaItemCommand(id), ct);
            return this.FromResult(result);
        }

        [HttpPost("{id:int}/thumbnail")]
        [Authorize(Policy = AppPermissions.Biblioteca.Update)]
        public async Task<IActionResult> UploadThumbnail(int id, IFormFile file, CancellationToken ct)
        {
            if (file is null || file.Length == 0)
                return BadRequest(ApiResponse<string>.Fail("No se recibió ningún archivo."));

            await using var stream = file.OpenReadStream();
            var result = await mediator.Send(new UploadMediaItemThumbnailCommand
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
