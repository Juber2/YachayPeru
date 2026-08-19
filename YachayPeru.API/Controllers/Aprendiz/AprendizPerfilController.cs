using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YachayPeru.API.Contracts.Aprendiz.Perfil.Request;
using YachayPeru.API.Contracts.Aprendiz.Perfil.Response;
using YachayPeru.API.Contracts.Common;
using YachayPeru.API.Authorization;
using YachayPeru.API.Extensions;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Application.Features.Aprendiz.Perfil.Commands.UpdateFavoriteRegion;
using YachayPeru.Application.Features.Aprendiz.Perfil.Queries.GetActividad;
using YachayPeru.Application.Features.Aprendiz.Perfil.Queries.GetResumen;

namespace YachayPeru.API.Controllers.Aprendiz
{
    [ApiController]
    [Route("aprendiz/perfil")]
    [Authorize(Policy = AppPermissions.AprendizPerfil.Read)]
    public class AprendizPerfilController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ICurrentUser currentUser;

        public AprendizPerfilController(IMediator _mediator, ICurrentUser _currentUser)
        {
            mediator = _mediator;
            currentUser = _currentUser;
        }

        [HttpGet("resumen")]
        public async Task<IActionResult> GetResumen(CancellationToken ct)
        {
            var d = await mediator.Send(new GetResumenQuery(currentUser.Id), ct);
            return Ok(ApiResponse<AprendizPerfilResumenResponse>.Ok(new AprendizPerfilResumenResponse
            {
                FullName = d.FullName,
                AvatarUrl = d.AvatarUrl,
                Level = d.Level,
                Points = d.Points,
                NextLevelPoints = d.NextLevelPoints,
                BadgeCount = d.BadgeCount,
                UltimaActividad = d.UltimaActividad is null ? null : new UltimaActividadResponse
                {
                    RegionId = d.UltimaActividad.RegionId,
                    RegionTitle = d.UltimaActividad.RegionTitle,
                    ModuleId = d.UltimaActividad.ModuleId,
                    ModuleTitle = d.UltimaActividad.ModuleTitle
                },
                IsPremiumUser = d.IsPremiumUser,
                ModulesDone = d.ModulesDone,
                LearningTimeMinutes = d.LearningTimeMinutes,
                FavoriteRegionId = d.FavoriteRegionId,
                FavoriteRegionTitle = d.FavoriteRegionTitle
            }));
        }

        [HttpGet("actividad")]
        public async Task<IActionResult> GetActividad(CancellationToken ct)
        {
            var items = await mediator.Send(new GetActividadQuery(currentUser.Id), ct);
            var response = items.Select(i => new AprendizActividadItemResponse
            {
                Text = i.Text,
                When = i.When
            }).ToList();
            return Ok(ApiResponse<List<AprendizActividadItemResponse>>.Ok(response));
        }

        [HttpPut("region-favorita")]
        public async Task<IActionResult> UpdateFavoriteRegion([FromBody] UpdateFavoriteRegionRequest request, CancellationToken ct)
        {
            var result = await mediator.Send(new UpdateFavoriteRegionCommand
            {
                UserId = currentUser.Id,
                RegionId = request.RegionId
            }, ct);
            return this.FromResult(result);
        }
    }
}
