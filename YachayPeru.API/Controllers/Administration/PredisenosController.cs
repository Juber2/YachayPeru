using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YachayPeru.API.Authorization;
using YachayPeru.API.Contracts.Administration.Predisenos.Request;
using YachayPeru.API.Contracts.Administration.Predisenos.Response;
using YachayPeru.API.Contracts.Common;
using YachayPeru.API.Extensions;
using YachayPeru.Application.Features.Predisenos.Commands.CreatePrediseno;
using YachayPeru.Application.Features.Predisenos.Commands.DeletePrediseno;
using YachayPeru.Application.Features.Predisenos.Commands.EditPrediseno;
using YachayPeru.Application.Features.Predisenos.Queries.GetPredisenoById;
using YachayPeru.Application.Features.Predisenos.Queries.GetPredisenos;

namespace YachayPeru.API.Controllers.Administration
{
    [ApiController]
    [Route("administration/predisenos")]
    [Authorize]
    public class PredisenosController : ControllerBase
    {
        private readonly IMediator mediator;

        public PredisenosController(IMediator _mediator) => mediator = _mediator;

        [HttpGet]
        [Authorize(Policy = AppPermissions.Predisenos.Read)]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var items = await mediator.Send(new GetPredisenosQuery(), ct);
            var response = items.Select(i => new PredisenoListItemResponse
            {
                Id = i.Id,
                Title = i.Title,
                TreeJson = i.TreeJson,
                CreatedAt = i.CreatedAt
            }).ToList();
            return Ok(ApiResponse<List<PredisenoListItemResponse>>.Ok(response));
        }

        [HttpGet("{id:int}")]
        [Authorize(Policy = AppPermissions.Predisenos.Read)]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var result = await mediator.Send(new GetPredisenoByIdQuery(id), ct);
            if (!result.IsSuccess) return this.FromResult(result);

            var d = result.Value!;
            return Ok(ApiResponse<PredisenoDetailResponse>.Ok(new PredisenoDetailResponse
            {
                Id = d.Id,
                Title = d.Title,
                TreeJson = d.TreeJson
            }));
        }

        [HttpPost]
        [Authorize(Policy = AppPermissions.Predisenos.Create)]
        public async Task<IActionResult> Create([FromBody] UpsertPredisenoRequest request, CancellationToken ct)
        {
            var result = await mediator.Send(new CreatePredisenoCommand
            {
                Title = request.Title,
                TreeJson = request.TreeJson
            }, ct);
            return this.FromResult(result);
        }

        [HttpPut("{id:int}")]
        [Authorize(Policy = AppPermissions.Predisenos.Update)]
        public async Task<IActionResult> Edit(int id, [FromBody] UpsertPredisenoRequest request, CancellationToken ct)
        {
            var result = await mediator.Send(new EditPredisenoCommand
            {
                Id = id,
                Title = request.Title,
                TreeJson = request.TreeJson
            }, ct);
            return this.FromResult(result);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = AppPermissions.Predisenos.Delete)]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var result = await mediator.Send(new DeletePredisenoCommand(id), ct);
            return this.FromResult(result);
        }
    }
}
