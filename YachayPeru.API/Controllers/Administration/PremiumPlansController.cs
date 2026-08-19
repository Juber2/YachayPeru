using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YachayPeru.API.Authorization;
using YachayPeru.API.Contracts.Administration.Premium.Request;
using YachayPeru.API.Contracts.Administration.Premium.Response;
using YachayPeru.API.Contracts.Common;
using YachayPeru.API.Extensions;
using YachayPeru.Application.Features.Administration.PremiumPlans.Commands.CreatePlan;
using YachayPeru.Application.Features.Administration.PremiumPlans.Commands.DeletePlan;
using YachayPeru.Application.Features.Administration.PremiumPlans.Commands.EditPlan;
using YachayPeru.Application.Features.Administration.PremiumPlans.Queries.GetPlanById;
using YachayPeru.Application.Features.Administration.PremiumPlans.Queries.GetPlans;

namespace YachayPeru.API.Controllers.Administration
{
    [ApiController]
    [Route("administration/premium/planes")]
    [Authorize]
    public class PremiumPlansController : ControllerBase
    {
        private readonly IMediator mediator;

        public PremiumPlansController(IMediator _mediator) => mediator = _mediator;

        [HttpGet]
        [Authorize(Policy = AppPermissions.Premium.Read)]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var result = await mediator.Send(new GetPlansQuery(), ct);
            if (!result.IsSuccess) return this.FromResult(result);

            var response = result.Value!.Select(ToResponse).ToList();
            return Ok(ApiResponse<List<PremiumPlanResponse>>.Ok(response));
        }

        [HttpGet("{id:int}")]
        [Authorize(Policy = AppPermissions.Premium.Read)]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var result = await mediator.Send(new GetPlanByIdQuery(id), ct);
            if (!result.IsSuccess) return this.FromResult(result);

            return Ok(ApiResponse<PremiumPlanResponse>.Ok(ToResponse(result.Value!)));
        }

        [HttpPost]
        [Authorize(Policy = AppPermissions.Premium.Create)]
        public async Task<IActionResult> Create([FromBody] UpsertPremiumPlanRequest request, CancellationToken ct)
        {
            var result = await mediator.Send(new CreatePlanCommand
            {
                Name = request.Name,
                Price = request.Price,
                IsActive = request.IsActive,
                FeatureBenefitIds = request.FeatureBenefitIds
            }, ct);
            return this.FromResult(result);
        }

        [HttpPut("{id:int}")]
        [Authorize(Policy = AppPermissions.Premium.Update)]
        public async Task<IActionResult> Edit(int id, [FromBody] UpsertPremiumPlanRequest request, CancellationToken ct)
        {
            var result = await mediator.Send(new EditPlanCommand
            {
                Id = id,
                Name = request.Name,
                Price = request.Price,
                IsActive = request.IsActive,
                FeatureBenefitIds = request.FeatureBenefitIds
            }, ct);
            return this.FromResult(result);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = AppPermissions.Premium.Delete)]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var result = await mediator.Send(new DeletePlanCommand(id), ct);
            return this.FromResult(result);
        }

        private static PremiumPlanResponse ToResponse(Application.Features.Administration.PremiumPlans.Queries.GetPlans.PremiumPlanDto d) => new()
        {
            Id = d.Id,
            Name = d.Name,
            Price = d.Price,
            IsActive = d.IsActive,
            Features = d.Features.Select(b => new PremiumBenefitResponse { Id = b.Id, Code = b.Code, Label = b.Label, Description = b.Description }).ToList()
        };
    }
}
