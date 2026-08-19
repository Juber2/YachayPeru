using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YachayPeru.API.Contracts.Aprendiz.Biblioteca.Response;
using YachayPeru.API.Contracts.Common;
using YachayPeru.API.Authorization;
using YachayPeru.API.Extensions;
using YachayPeru.Application.Features.Aprendiz.Biblioteca.Queries.GetMediaItemById;
using YachayPeru.Application.Features.Aprendiz.Biblioteca.Queries.GetMediaItems;

namespace YachayPeru.API.Controllers.Aprendiz
{
    [ApiController]
    [Route("aprendiz/biblioteca")]
    [Authorize(Policy = AppPermissions.AprendizBiblioteca.Read)]
    public class AprendizBibliotecaController : ControllerBase
    {
        private readonly IMediator mediator;

        public AprendizBibliotecaController(IMediator _mediator)
        {
            mediator = _mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? mediaType, [FromQuery] int? regionId, CancellationToken ct)
        {
            var items = await mediator.Send(new GetMediaItemsQuery(mediaType, regionId), ct);
            var response = items.Select(m => new AprendizMediaItemListItemResponse
            {
                Id = m.Id,
                Title = m.Title,
                MediaTypeCode = m.MediaTypeCode,
                RegionId = m.RegionId,
                RegionTitle = m.RegionTitle,
                ThumbnailUrl = Request.ToAbsoluteUrl(m.ThumbnailUrl),
                IsPlayable = m.IsPlayable
            }).ToList();
            return Ok(ApiResponse<List<AprendizMediaItemListItemResponse>>.Ok(response));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var result = await mediator.Send(new GetMediaItemByIdQuery(id), ct);
            if (!result.IsSuccess) return this.FromResult(result);

            var d = result.Value!;
            return Ok(ApiResponse<AprendizMediaItemDetailResponse>.Ok(new AprendizMediaItemDetailResponse
            {
                Id = d.Id,
                Title = d.Title,
                MediaTypeCode = d.MediaTypeCode,
                RegionId = d.RegionId,
                RegionTitle = d.RegionTitle,
                ThumbnailUrl = Request.ToAbsoluteUrl(d.ThumbnailUrl),
                ExternalUrl = d.ExternalUrl,
                LegendText = d.LegendText,
                IsPlayable = d.IsPlayable
            }));
        }
    }
}
