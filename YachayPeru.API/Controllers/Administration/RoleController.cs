using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YachayPeru.API.Authorization;
using YachayPeru.API.Contracts.Administration.Roles.Request;
using YachayPeru.API.Contracts.Common;
using YachayPeru.API.Extensions;
using YachayPeru.Application.Features.Administration.Roles.Commands.CreateRole;
using YachayPeru.Application.Features.Administration.Roles.Commands.DeleteRole;
using YachayPeru.Application.Features.Administration.Roles.Commands.EditRole;
using YachayPeru.Application.Features.Administration.Roles.Queries.GetRoleDetail;
using YachayPeru.Application.Features.Administration.Roles.Queries.GetRoleList;
using YachayPeru.Application.Features.Administration.Roles.Queries.GetRoleLookup;
using YachayPeru.Application.Features.Permissions.Queries.GetPermissionsMatrix;

namespace YachayPeru.API.Controllers.Administration
{
    [ApiController]
    [Route("administration")]
    [Authorize]
    public class RoleController : ControllerBase
    {
        private readonly IMediator mediator;

        public RoleController(IMediator _mediator)
        {
            mediator = _mediator;
        }

        [HttpGet("roles")]
        [Authorize(Policy = AppPermissions.RolesApoyo.Read)]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var result = await mediator.Send(new GetRoleListQuery(), ct);
            return this.FromResult(result);
        }

        [HttpGet("roles/lookup")]
        [Authorize(Policy = AppPermissions.RolesApoyo.Read)]
        public async Task<IActionResult> GetLookup(CancellationToken ct)
        {
            var result = await mediator.Send(new GetRoleLookupQuery(), ct);
            return this.FromResult(result);
        }

        [HttpGet("roles/{id:int}")]
        [Authorize(Policy = AppPermissions.RolesApoyo.Read)]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var result = await mediator.Send(new GetRoleDetailQuery(id), ct);
            return this.FromResult(result);
        }

        [HttpGet("permissions/matrix")]
        [Authorize(Policy = AppPermissions.RolesApoyo.Read)]
        public async Task<IActionResult> GetPermissionsMatrix(CancellationToken ct)
        {
            var result = await mediator.Send(new GetPermissionsMatrixQuery(), ct);
            return this.FromResult(result);
        }

        [HttpPost("roles")]
        [Authorize(Policy = AppPermissions.RolesApoyo.Create)]
        public async Task<IActionResult> Create([FromBody] CreateRoleRequest request, CancellationToken ct)
        {
            var command = new CreateRoleCommand
            {
                Name = request.Name,
                RoleCode = request.RoleCode,
                Description = request.Description,
                PermissionIds = request.PermissionIds
            };

            var result = await mediator.Send(command, ct);
            return this.FromResult(result);
        }

        [HttpPut("roles/{id:int}")]
        [Authorize(Policy = AppPermissions.RolesApoyo.Update)]
        public async Task<IActionResult> Edit(int id, [FromBody] EditRoleRequest request, CancellationToken ct)
        {
            var command = new EditRoleCommand
            {
                Id = id,
                Name = request.Name,
                RoleCode = request.RoleCode,
                Description = request.Description,
                PermissionIds = request.PermissionIds
            };

            var result = await mediator.Send(command, ct);
            return this.FromResult(result);
        }

        [HttpDelete("roles/{id:int}")]
        [Authorize(Policy = AppPermissions.RolesApoyo.Delete)]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var result = await mediator.Send(new DeleteRoleCommand(id), ct);
            return this.FromResult(result);
        }
    }
}
