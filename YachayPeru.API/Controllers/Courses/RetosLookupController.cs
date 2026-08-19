using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YachayPeru.API.Authorization;
using YachayPeru.API.Contracts.Common;
using YachayPeru.API.Contracts.Retos.Response;
using YachayPeru.Application.Features.Retos.Queries.GetRetosLookup;

namespace YachayPeru.API.Controllers.Courses
{
    [ApiController]
    [Route("retos")]
    [Authorize]
    public class RetosLookupController : ControllerBase
    {
        private readonly IMediator mediator;

        public RetosLookupController(IMediator _mediator) => mediator = _mediator;

        [HttpGet("lookup")]
        [Authorize(Policy = AppPermissions.CursosPlantilla.Read)]
        public async Task<IActionResult> GetLookup(CancellationToken ct)
        {
            var items = await mediator.Send(new GetRetosLookupQuery(), ct);
            var response = items.Select(r => new RetoLookupItemResponse
            {
                Id          = r.Id,
                Title       = r.Title,
                CourseId    = r.CourseId,
                RegionTitle = r.RegionTitle
            }).ToList();
            return Ok(ApiResponse<List<RetoLookupItemResponse>>.Ok(response));
        }
    }
}
