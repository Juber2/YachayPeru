using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YachayPeru.API.Contracts.Aprendiz.Insignias.Response;
using YachayPeru.API.Authorization;
using YachayPeru.API.Contracts.Common;
using YachayPeru.API.Extensions;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Application.Features.Aprendiz.Insignias.Queries.GetInsignias;

namespace YachayPeru.API.Controllers.Aprendiz
{
    [ApiController]
    [Route("aprendiz/insignias")]
    [Authorize(Policy = AppPermissions.AprendizInsignias.Read)]
    public class AprendizInsigniasController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ICurrentUser currentUser;

        public AprendizInsigniasController(IMediator _mediator, ICurrentUser _currentUser)
        {
            mediator = _mediator;
            currentUser = _currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var items = await mediator.Send(new GetInsigniasQuery(currentUser.Id), ct);
            var response = items.Select(i => new AprendizInsigniaListItemResponse
            {
                Id = i.Id,
                Name = i.Name,
                Description = i.Description,
                ImageUrl = Request.ToAbsoluteUrl(i.ImageUrl),
                IsEarned = i.IsEarned,
                EarnedAt = i.EarnedAt
            }).ToList();
            return Ok(ApiResponse<List<AprendizInsigniaListItemResponse>>.Ok(response));
        }
    }
}
