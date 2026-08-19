using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YachayPeru.API.Contracts.Aprendiz.Comunidad.Response;
using YachayPeru.API.Contracts.Common;
using YachayPeru.API.Authorization;
using YachayPeru.API.Extensions;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Application.Features.Aprendiz.Comunidad.Commands.PostLike;
using YachayPeru.Application.Features.Aprendiz.Comunidad.Queries.GetPosts;

namespace YachayPeru.API.Controllers.Aprendiz
{
    [ApiController]
    [Route("aprendiz/comunidad")]
    [Authorize(Policy = AppPermissions.AprendizComunidad.Read)]
    public class AprendizComunidadController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ICurrentUser currentUser;

        public AprendizComunidadController(IMediator _mediator, ICurrentUser _currentUser)
        {
            mediator = _mediator;
            currentUser = _currentUser;
        }

        [HttpGet("posts")]
        public async Task<IActionResult> GetPosts(CancellationToken ct)
        {
            var items = await mediator.Send(new GetPostsQuery(currentUser.Id), ct);
            var response = items.Select(p => new AprendizCommunityPostResponse
            {
                Id = p.Id,
                AuthorName = p.AuthorName,
                AuthorInitials = p.AuthorInitials,
                RegionId = p.RegionId,
                Text = p.Text,
                PhotoUrl = Request.ToAbsoluteUrl(p.PhotoUrl),
                LikeCount = p.LikeCount,
                LikedByMe = p.LikedByMe,
                CreatedAt = p.CreatedAt
            }).ToList();
            return Ok(ApiResponse<List<AprendizCommunityPostResponse>>.Ok(response));
        }

        [HttpPost("posts/{id:int}/like")]
        public async Task<IActionResult> ToggleLike(int id, CancellationToken ct)
        {
            var result = await mediator.Send(new PostLikeCommand(currentUser.Id, id), ct);
            if (!result.IsSuccess) return this.FromResult(result);

            var d = result.Value!;
            return Ok(ApiResponse<AprendizPostLikeResponse>.Ok(new AprendizPostLikeResponse
            {
                LikedByMe = d.LikedByMe,
                LikeCount = d.LikeCount
            }));
        }
    }
}
