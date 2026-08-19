using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YachayPeru.API.Authorization;
using YachayPeru.API.Contracts.Common;
using YachayPeru.API.Contracts.Administration.RegionDestacada.Request;
using YachayPeru.API.Contracts.Administration.RegionDestacada.Response;
using YachayPeru.API.Extensions;
using YachayPeru.Application.Features.RegionDestacada.Commands.CreateRegionDestacada;
using YachayPeru.Application.Features.RegionDestacada.Commands.DeleteRegionDestacada;
using YachayPeru.Application.Features.RegionDestacada.Commands.EditRegionDestacada;
using YachayPeru.Application.Features.RegionDestacada.Queries.GetRegionDestacadaById;
using YachayPeru.Application.Features.RegionDestacada.Queries.GetRegionesDestacadas;

namespace YachayPeru.API.Controllers.Administration
{
    [ApiController]
    [Route("administration/region-destacada")]
    [Authorize]
    public class RegionDestacadaController : ControllerBase
    {
        private readonly IMediator mediator;

        public RegionDestacadaController(IMediator _mediator) => mediator = _mediator;

        [HttpGet]
        [Authorize(Policy = AppPermissions.RegionDestacada.Read)]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var items = await mediator.Send(new GetRegionesDestacadasQuery(), ct);
            var response = items.Select(i => new RegionDestacadaListItemResponse
            {
                Id = i.Id,
                RegionId = i.RegionId,
                RegionTitle = i.RegionTitle,
                StartDate = i.StartDate,
                EndDate = i.EndDate
            }).ToList();
            return Ok(ApiResponse<List<RegionDestacadaListItemResponse>>.Ok(response));
        }

        [HttpGet("{id:int}")]
        [Authorize(Policy = AppPermissions.RegionDestacada.Read)]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var result = await mediator.Send(new GetRegionDestacadaByIdQuery(id), ct);
            if (!result.IsSuccess) return this.FromResult(result);

            var d = result.Value!;
            return Ok(ApiResponse<RegionDestacadaDetailResponse>.Ok(new RegionDestacadaDetailResponse
            {
                Id = d.Id,
                RegionId = d.RegionId,
                RegionTitle = d.RegionTitle,
                StartDate = d.StartDate,
                EndDate = d.EndDate
            }));
        }

        [HttpPost]
        [Authorize(Policy = AppPermissions.RegionDestacada.Create)]
        public async Task<IActionResult> Create([FromBody] UpsertRegionDestacadaRequest request, CancellationToken ct)
        {
            var result = await mediator.Send(new CreateRegionDestacadaCommand
            {
                RegionId = request.RegionId,
                StartDate = request.StartDate,
                EndDate = request.EndDate
            }, ct);
            return this.FromResult(result);
        }

        [HttpPut("{id:int}")]
        [Authorize(Policy = AppPermissions.RegionDestacada.Update)]
        public async Task<IActionResult> Edit(int id, [FromBody] UpsertRegionDestacadaRequest request, CancellationToken ct)
        {
            var result = await mediator.Send(new EditRegionDestacadaCommand
            {
                Id = id,
                RegionId = request.RegionId,
                StartDate = request.StartDate,
                EndDate = request.EndDate
            }, ct);
            return this.FromResult(result);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = AppPermissions.RegionDestacada.Delete)]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var result = await mediator.Send(new DeleteRegionDestacadaCommand(id), ct);
            return this.FromResult(result);
        }
    }
}
