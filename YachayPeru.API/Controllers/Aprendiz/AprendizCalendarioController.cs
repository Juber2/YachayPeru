using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YachayPeru.API.Contracts.Aprendiz.Calendario.Request;
using YachayPeru.API.Contracts.Aprendiz.Calendario.Response;
using YachayPeru.API.Contracts.Common;
using YachayPeru.API.Authorization;
using YachayPeru.API.Extensions;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Application.Features.Aprendiz.Calendario.Commands.PutRecordatorio;
using YachayPeru.Application.Features.Aprendiz.Calendario.Queries.GetFestividades;
using YachayPeru.Application.Features.Aprendiz.Calendario.Queries.GetProximaFestividad;

namespace YachayPeru.API.Controllers.Aprendiz
{
    [ApiController]
    [Route("aprendiz/calendario")]
    [Authorize(Policy = AppPermissions.AprendizCalendario.Read)]
    public class AprendizCalendarioController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ICurrentUser currentUser;

        public AprendizCalendarioController(IMediator _mediator, ICurrentUser _currentUser)
        {
            mediator = _mediator;
            currentUser = _currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var items = await mediator.Send(new GetFestividadesQuery(currentUser.Id), ct);
            var response = items.Select(f => new AprendizFestividadListItemResponse
            {
                Id = f.Id,
                Name = f.Name,
                Description = f.Description,
                RegionId = f.RegionId,
                RegionTitle = f.RegionTitle,
                Month = f.Month,
                Day = f.Day,
                IsReminderOn = f.IsReminderOn
            }).ToList();
            return Ok(ApiResponse<List<AprendizFestividadListItemResponse>>.Ok(response));
        }

        [HttpGet("proxima")]
        public async Task<IActionResult> GetProxima(CancellationToken ct)
        {
            var d = await mediator.Send(new GetProximaFestividadQuery(currentUser.Id), ct);
            if (d is null)
                return Ok(ApiResponse<AprendizProximaFestividadResponse?>.Ok(null));

            return Ok(ApiResponse<AprendizProximaFestividadResponse?>.Ok(new AprendizProximaFestividadResponse
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description,
                RegionId = d.RegionId,
                RegionTitle = d.RegionTitle,
                Month = d.Month,
                Day = d.Day,
                DaysUntil = d.DaysUntil,
                IsReminderOn = d.IsReminderOn
            }));
        }

        [HttpPut("{id:int}/recordatorio")]
        public async Task<IActionResult> PutRecordatorio(int id, [FromBody] PutRecordatorioRequest request, CancellationToken ct)
        {
            var result = await mediator.Send(new PutRecordatorioCommand
            {
                UserId = currentUser.Id,
                FestividadId = id,
                Enabled = request.Enabled
            }, ct);
            return this.FromResult(result);
        }
    }
}
