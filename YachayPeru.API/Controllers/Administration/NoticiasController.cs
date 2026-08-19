using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YachayPeru.API.Authorization;
using YachayPeru.API.Contracts.Common;
using YachayPeru.API.Contracts.Administration.Noticias.Request;
using YachayPeru.API.Contracts.Administration.Noticias.Response;
using YachayPeru.API.Extensions;
using YachayPeru.Application.Features.Noticias.Commands.CreateNoticia;
using YachayPeru.Application.Features.Noticias.Commands.DeleteNoticia;
using YachayPeru.Application.Features.Noticias.Commands.EditNoticia;
using YachayPeru.Application.Features.Noticias.Commands.UploadNoticiaImage;
using YachayPeru.Application.Features.Noticias.Queries.GetNoticiaById;
using YachayPeru.Application.Features.Noticias.Queries.GetNoticias;

namespace YachayPeru.API.Controllers.Administration
{
    [ApiController]
    [Route("administration/noticias")]
    [Authorize]
    public class NoticiasController : ControllerBase
    {
        private readonly IMediator mediator;

        public NoticiasController(IMediator _mediator) => mediator = _mediator;

        [HttpGet]
        [Authorize(Policy = AppPermissions.Noticias.Read)]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var items = await mediator.Send(new GetNoticiasQuery(), ct);
            var response = items.Select(n => new NoticiaListItemResponse
            {
                Id = n.Id,
                Title = n.Title,
                Category = n.Category,
                ImageUrl = Request.ToAbsoluteUrl(n.ImageUrl),
                IsActive = n.IsActive,
                CreatedAt = n.CreatedAt
            }).ToList();
            return Ok(ApiResponse<List<NoticiaListItemResponse>>.Ok(response));
        }

        [HttpGet("{id:int}")]
        [Authorize(Policy = AppPermissions.Noticias.Read)]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var result = await mediator.Send(new GetNoticiaByIdQuery(id), ct);
            if (!result.IsSuccess) return this.FromResult(result);

            var d = result.Value!;
            return Ok(ApiResponse<NoticiaDetailResponse>.Ok(new NoticiaDetailResponse
            {
                Id = d.Id,
                Title = d.Title,
                Category = d.Category,
                Body = d.Body,
                ImageUrl = Request.ToAbsoluteUrl(d.ImageUrl),
                IsActive = d.IsActive
            }));
        }

        [HttpPost]
        [Authorize(Policy = AppPermissions.Noticias.Create)]
        public async Task<IActionResult> Create([FromBody] UpsertNoticiaRequest request, CancellationToken ct)
        {
            var result = await mediator.Send(new CreateNoticiaCommand
            {
                Title = request.Title,
                Category = request.Category,
                Body = request.Body,
                IsActive = request.IsActive
            }, ct);
            return this.FromResult(result);
        }

        [HttpPut("{id:int}")]
        [Authorize(Policy = AppPermissions.Noticias.Update)]
        public async Task<IActionResult> Edit(int id, [FromBody] UpsertNoticiaRequest request, CancellationToken ct)
        {
            var result = await mediator.Send(new EditNoticiaCommand
            {
                Id = id,
                Title = request.Title,
                Category = request.Category,
                Body = request.Body,
                IsActive = request.IsActive
            }, ct);
            return this.FromResult(result);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = AppPermissions.Noticias.Delete)]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var result = await mediator.Send(new DeleteNoticiaCommand(id), ct);
            return this.FromResult(result);
        }

        [HttpPost("{id:int}/image")]
        [Authorize(Policy = AppPermissions.Noticias.Update)]
        public async Task<IActionResult> UploadImage(int id, IFormFile file, CancellationToken ct)
        {
            if (file is null || file.Length == 0)
                return BadRequest(ApiResponse<string>.Fail("No se recibió ningún archivo."));

            await using var stream = file.OpenReadStream();
            var result = await mediator.Send(new UploadNoticiaImageCommand
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
