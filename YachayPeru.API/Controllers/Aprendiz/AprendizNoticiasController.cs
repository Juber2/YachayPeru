using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YachayPeru.API.Authorization;
using YachayPeru.API.Contracts.Aprendiz.Noticias.Response;
using YachayPeru.API.Contracts.Common;
using YachayPeru.API.Extensions;
using YachayPeru.Application.Features.Aprendiz.Noticias.Queries.GetNoticiaById;
using YachayPeru.Application.Features.Aprendiz.Noticias.Queries.GetNoticias;

namespace YachayPeru.API.Controllers.Aprendiz
{
    [ApiController]
    [Route("aprendiz/noticias")]
    [Authorize(Policy = AppPermissions.AprendizNoticias.Read)]
    public class AprendizNoticiasController : ControllerBase
    {
        private readonly IMediator mediator;

        public AprendizNoticiasController(IMediator _mediator) => mediator = _mediator;

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var items = await mediator.Send(new GetNoticiasQuery(), ct);
            var response = items.Select(n => new AprendizNoticiaListItemResponse
            {
                Id = n.Id,
                Title = n.Title,
                Category = n.Category,
                ImageUrl = Request.ToAbsoluteUrl(n.ImageUrl),
                CreatedAt = n.CreatedAt,
                Body=n.Body
            }).ToList();
            return Ok(ApiResponse<List<AprendizNoticiaListItemResponse>>.Ok(response));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var result = await mediator.Send(new GetNoticiaByIdQuery(id), ct);
            if (!result.IsSuccess) return this.FromResult(result);

            var d = result.Value!;
            return Ok(ApiResponse<AprendizNoticiaDetailResponse>.Ok(new AprendizNoticiaDetailResponse
            {
                Id = d.Id,
                Title = d.Title,
                Category = d.Category,
                Body = d.Body,
                ImageUrl = Request.ToAbsoluteUrl(d.ImageUrl),
                CreatedAt = d.CreatedAt
            }));
        }
    }
}
