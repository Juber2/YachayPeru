using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YachayPeru.API.Contracts.Common;
using YachayPeru.Application.Features.MasterCodes.Queries.GetMasterCodesByParent;
using YachayPeru.Application.Features.Resources.Queries.GetResourcesByScope;

namespace YachayPeru.API.Controllers.Common
{
    [ApiController]
    [Route("common")]
    [Authorize]
    public class MasterCodesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MasterCodesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("master-codes")]
        public async Task<IActionResult> GetByParent([FromQuery] string parentCode, CancellationToken ct)
        {
            var items = await _mediator.Send(new GetMasterCodesByParentQuery(parentCode), ct);
            return Ok(ApiResponse<IReadOnlyList<MasterCodeItem>>.Ok(items));
        }

        [HttpGet("resources")]
        public async Task<IActionResult> GetResources([FromQuery] string scope, CancellationToken ct)
        {
            var items = await _mediator.Send(new GetResourcesByScopeQuery(scope), ct);
            return Ok(ApiResponse<IReadOnlyList<ResourcePermissionItem>>.Ok(items));
        }
    }
}
