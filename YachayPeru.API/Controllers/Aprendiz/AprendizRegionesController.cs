using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YachayPeru.API.Contracts.Aprendiz.Regiones.Response;
using YachayPeru.API.Contracts.Common;
using YachayPeru.API.Authorization;
using YachayPeru.API.Extensions;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Application.Features.Aprendiz.Regiones.Queries.GetRegionById;
using YachayPeru.Application.Features.Aprendiz.Regiones.Queries.GetRegiones;

namespace YachayPeru.API.Controllers.Aprendiz
{
    [ApiController]
    [Route("aprendiz/regiones")]
    [Authorize(Policy = AppPermissions.AprendizRegiones.Read)]
    public class AprendizRegionesController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ICurrentUser currentUser;

        public AprendizRegionesController(IMediator _mediator, ICurrentUser _currentUser)
        {
            mediator = _mediator;
            currentUser = _currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var items = await mediator.Send(new GetRegionesQuery(currentUser.Id), ct);
            var response = items.Select(r => new AprendizRegionListItemResponse
            {
                Id = r.Id,
                Title = r.Title,
                Description = r.Description,
                CoverImageUrl = Request.ToAbsoluteUrl(r.CoverImageUrl),
                ProgressPercent = r.ProgressPercent,
                IsCompleted = r.IsCompleted
            }).ToList();
            return Ok(ApiResponse<List<AprendizRegionListItemResponse>>.Ok(response));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var result = await mediator.Send(new GetRegionByIdQuery(currentUser.Id, id), ct);
            if (!result.IsSuccess) return this.FromResult(result);

            var d = result.Value!;
            return Ok(ApiResponse<AprendizRegionDetailResponse>.Ok(new AprendizRegionDetailResponse
            {
                Id = d.Id,
                Title = d.Title,
                Description = d.Description,
                CoverImageUrl = Request.ToAbsoluteUrl(d.CoverImageUrl),
                AmbientAudioUrl = Request.ToAbsoluteUrl(d.AmbientAudioUrl),
                AmbientAudioTitle = d.AmbientAudioTitle,
                SpotifyUrl = d.SpotifyUrl,
                Modules = d.Modules.Select(m => new AprendizModuleResponse
                {
                    Id = m.Id,
                    Title = m.Title,
                    Description = m.Description,
                    OrderIndex = m.OrderIndex,
                    DurationHours = m.DurationHours,
                    Contents = m.Contents.Select(c => new AprendizModuleContentResponse
                    {
                        Id = c.Id,
                        Text = c.Text,
                        OrderIndex = c.OrderIndex,
                        Files = c.Files.Select(f => new AprendizModuleContentFileResponse
                        {
                            Id = f.Id,
                            FileTypeCode = f.FileTypeCode,
                            FileUrl = Request.ToAbsoluteUrl(f.FileUrl)!,
                            FileName = f.FileName,
                            OrderIndex = f.OrderIndex
                        }).ToList()
                    }).ToList()
                }).ToList(),
                RetoCount = d.RetoCount,
                CompletedRetoCount = d.CompletedRetoCount
            }));
        }
    }
}
