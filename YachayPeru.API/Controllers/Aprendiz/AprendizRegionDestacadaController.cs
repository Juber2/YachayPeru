using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YachayPeru.API.Authorization;
using YachayPeru.API.Contracts.Aprendiz.RegionDestacada.Response;
using YachayPeru.API.Contracts.Common;
using YachayPeru.API.Extensions;
using YachayPeru.Application.Features.Aprendiz.RegionDestacada.Queries.GetRegionDestacadaActual;

namespace YachayPeru.API.Controllers.Aprendiz
{
    [ApiController]
    [Route("aprendiz/region-destacada")]
    [Authorize(Policy = AppPermissions.AprendizRegionDestacada.Read)]
    public class AprendizRegionDestacadaController : ControllerBase
    {
        private readonly IMediator mediator;

        public AprendizRegionDestacadaController(IMediator _mediator) => mediator = _mediator;

        [HttpGet]
        public async Task<IActionResult> GetActual(CancellationToken ct)
        {
            var d = await mediator.Send(new GetRegionDestacadaActualQuery(), ct);
            if (d is null)
                return Ok(ApiResponse<AprendizRegionDestacadaResponse?>.Ok(null));

            return Ok(ApiResponse<AprendizRegionDestacadaResponse?>.Ok(new AprendizRegionDestacadaResponse
            {
                RegionId = d.RegionId,
                RegionTitle = d.RegionTitle,
                RegionDescription = d.RegionDescription,
                CoverImageUrl = Request.ToAbsoluteUrl(d.CoverImageUrl),
                StartDate = d.StartDate,
                EndDate = d.EndDate
            }));
        }
    }
}
