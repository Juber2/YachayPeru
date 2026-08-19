using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YachayPeru.API.Authorization;
using YachayPeru.API.Contracts.Administration.Users.Request;
using YachayPeru.API.Contracts.Common;
using YachayPeru.API.Extensions;
using YachayPeru.API.Mappings;
using YachayPeru.Application.Common.Results;
using YachayPeru.Application.Features.Administration.Users.Commands.CreateUser;
using YachayPeru.Application.Features.Administration.Users.Commands.DeleteUser;
using YachayPeru.Application.Features.Administration.Users.Commands.EditUser;
using YachayPeru.Application.Features.Administration.Users.Queries.GetUserDetail;
using YachayPeru.Application.Features.Administration.Users.Queries.GetUserList;

namespace YachayPeru.API.Controllers.Administration
{
    [ApiController]
    [Route("administration")]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly AppMapper _mapper;

        public UsersController(IMediator mediator, AppMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;

        }

        [HttpGet("users")]
        [Authorize(Policy = AppPermissions.UsuariosApoyo.Read)]
        public async Task<IActionResult> GetList(CancellationToken ct)
        {
            var result = await _mediator.Send(new GetUserListQuery(), ct);
            return this.FromResult(result);
        }

        [HttpGet("users/{id:int}")]
        [Authorize(Policy = AppPermissions.UsuariosApoyo.Read)]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var result = await _mediator.Send(new GetUserDetailQuery(id), ct);
            return this.FromResult(result);
        }

        [HttpPost("users")]
        [Authorize(Policy = AppPermissions.UsuariosApoyo.Create)]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest request, CancellationToken ct)
        {
            var command = _mapper.ToCommand(request);
            var result = await _mediator.Send(command, ct);

            if (!result.IsSuccess && result.Code == ResultCodes.UserPreviouslyDeleted)
                return Conflict(new ApiResponse<object>
                {
                    Success = false,
                    Code = result.Code,
                    Message = result.Message!,
                    Data = result.Metadata
                });

            return this.FromResult(result);
        }

        [HttpPut("users/{id:int}")]
        [Authorize(Policy = AppPermissions.UsuariosApoyo.Update)]
        public async Task<IActionResult> Edit(int id, [FromBody] EditUserRequest request, CancellationToken ct)
        {
            var command = new EditUserCommand
            {
                Id = id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Username = request.UserName,
                Email = request.Email,
                Password = request.Password,
                IsLocked = request.IsLocked,
                RoleId = request.RoleId
            };

            var result = await _mediator.Send(command, ct);
            return this.FromResult(result);
        }

        [HttpDelete("users/{id:int}")]
        [Authorize(Policy = AppPermissions.UsuariosApoyo.Delete)]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var result = await _mediator.Send(new DeleteUserCommand(id), ct);
            return this.FromResult(result);
        }
    }
}
