using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YachayPeru.API.Authorization;
using YachayPeru.API.Contracts.Common;
using YachayPeru.API.Contracts.Administration.Calendario.Request;
using YachayPeru.API.Contracts.Administration.Calendario.Response;
using YachayPeru.API.Extensions;
using YachayPeru.Application.Features.Calendario.Commands.CreateFestividad;
using YachayPeru.Application.Features.Calendario.Commands.DeleteFestividad;
using YachayPeru.Application.Features.Calendario.Commands.EditFestividad;
using YachayPeru.Application.Features.Calendario.Queries.GetFestividadById;
using YachayPeru.Application.Features.Calendario.Queries.GetFestividades;

namespace YachayPeru.API.Controllers.Administration
{
    [ApiController]
    [Route("administration/calendario")]
    [Authorize]
    public class CalendarioController : ControllerBase
    {
        private readonly IMediator mediator;

        public CalendarioController(IMediator _mediator) => mediator = _mediator;

        [HttpGet]
        [Authorize(Policy = AppPermissions.Calendario.Read)]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var items = await mediator.Send(new GetFestividadesQuery(), ct);
            var response = items.Select(f => new FestividadListItemResponse
            {
                Id = f.Id,
                Name = f.Name,
                RegionId = f.RegionId,
                RegionTitle = f.RegionTitle,
                Month = f.Month,
                Day = f.Day,
                IsActive = f.IsActive,
                CreatedAt = f.CreatedAt
            }).ToList();
            return Ok(ApiResponse<List<FestividadListItemResponse>>.Ok(response));
        }

        [HttpGet("{id:int}")]
        [Authorize(Policy = AppPermissions.Calendario.Read)]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var result = await mediator.Send(new GetFestividadByIdQuery(id), ct);
            if (!result.IsSuccess) return this.FromResult(result);

            var d = result.Value!;
            return Ok(ApiResponse<FestividadDetailResponse>.Ok(new FestividadDetailResponse
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description,
                RegionId = d.RegionId,
                RegionTitle = d.RegionTitle,
                Month = d.Month,
                Day = d.Day,
                IsActive = d.IsActive
            }));
        }

        [HttpPost]
        [Authorize(Policy = AppPermissions.Calendario.Create)]
        public async Task<IActionResult> Create([FromBody] UpsertFestividadRequest request, CancellationToken ct)
        {
            var result = await mediator.Send(new CreateFestividadCommand
            {
                Name = request.Name,
                Description = request.Description,
                RegionId = request.RegionId,
                Month = request.Month,
                Day = request.Day,
                IsActive = request.IsActive
            }, ct);
            return this.FromResult(result);
        }

        [HttpPut("{id:int}")]
        [Authorize(Policy = AppPermissions.Calendario.Update)]
        public async Task<IActionResult> Edit(int id, [FromBody] UpsertFestividadRequest request, CancellationToken ct)
        {
            var result = await mediator.Send(new EditFestividadCommand
            {
                Id = id,
                Name = request.Name,
                Description = request.Description,
                RegionId = request.RegionId,
                Month = request.Month,
                Day = request.Day,
                IsActive = request.IsActive
            }, ct);
            return this.FromResult(result);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = AppPermissions.Calendario.Delete)]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var result = await mediator.Send(new DeleteFestividadCommand(id), ct);
            return this.FromResult(result);
        }
    }
}
