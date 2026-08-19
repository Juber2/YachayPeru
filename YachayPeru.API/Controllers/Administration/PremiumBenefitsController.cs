using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YachayPeru.API.Authorization;
using YachayPeru.API.Contracts.Administration.Premium.Request;
using YachayPeru.API.Contracts.Administration.Premium.Response;
using YachayPeru.API.Contracts.Common;
using YachayPeru.API.Extensions;
using YachayPeru.Application.Features.Administration.PremiumBenefits.Commands.CreateBenefit;
using YachayPeru.Application.Features.Administration.PremiumBenefits.Commands.DeleteBenefit;
using YachayPeru.Application.Features.Administration.PremiumBenefits.Commands.EditBenefit;
using YachayPeru.Application.Features.Administration.PremiumBenefits.Queries.GetBenefitById;
using YachayPeru.Application.Features.Administration.PremiumBenefits.Queries.GetBenefits;

namespace YachayPeru.API.Controllers.Administration
{
    [ApiController]
    [Route("administration/premium/beneficios")]
    [Authorize]
    public class PremiumBenefitsController : ControllerBase
    {
        private readonly IMediator mediator;

        public PremiumBenefitsController(IMediator _mediator) => mediator = _mediator;

        [HttpGet]
        [Authorize(Policy = AppPermissions.Premium.Read)]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var items = await mediator.Send(new GetBenefitsQuery(), ct);
            var response = items.Select(b => new PremiumBenefitResponse { Id = b.Id, Code = b.Code, Label = b.Label, Description = b.Description }).ToList();
            return Ok(ApiResponse<List<PremiumBenefitResponse>>.Ok(response));
        }

        [HttpGet("{id:int}")]
        [Authorize(Policy = AppPermissions.Premium.Read)]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var result = await mediator.Send(new GetBenefitByIdQuery(id), ct);
            if (!result.IsSuccess) return this.FromResult(result);

            var d = result.Value!;
            return Ok(ApiResponse<PremiumBenefitResponse>.Ok(new PremiumBenefitResponse { Id = d.Id, Code = d.Code, Label = d.Label, Description = d.Description }));
        }

        [HttpPost]
        [Authorize(Policy = AppPermissions.Premium.Create)]
        public async Task<IActionResult> Create([FromBody] UpsertPremiumBenefitRequest request, CancellationToken ct)
        {
            var result = await mediator.Send(new CreateBenefitCommand { Code = request.Code, Label = request.Label, Description = request.Description }, ct);
            return this.FromResult(result);
        }

        [HttpPut("{id:int}")]
        [Authorize(Policy = AppPermissions.Premium.Update)]
        public async Task<IActionResult> Edit(int id, [FromBody] UpsertPremiumBenefitRequest request, CancellationToken ct)
        {
            var result = await mediator.Send(new EditBenefitCommand { Id = id, Code = request.Code, Label = request.Label, Description = request.Description }, ct);
            return this.FromResult(result);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = AppPermissions.Premium.Delete)]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var result = await mediator.Send(new DeleteBenefitCommand(id), ct);
            return this.FromResult(result);
        }
    }
}
