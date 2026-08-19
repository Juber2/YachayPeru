using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YachayPeru.API.Authorization;
using YachayPeru.API.Contracts.Common;
using YachayPeru.API.Contracts.Administration.Insignias.Request;
using YachayPeru.API.Contracts.Administration.Insignias.Response;
using YachayPeru.API.Extensions;
using YachayPeru.Application.Features.Insignias.Commands.CreateInsignia;
using YachayPeru.Application.Features.Insignias.Commands.DeleteInsignia;
using YachayPeru.Application.Features.Insignias.Commands.EditInsignia;
using YachayPeru.Application.Features.Insignias.Commands.UploadInsigniaImage;
using YachayPeru.Application.Features.Insignias.Queries.GetInsigniaById;
using YachayPeru.Application.Features.Insignias.Queries.GetInsignias;

namespace YachayPeru.API.Controllers.Administration
{
    [ApiController]
    [Route("administration/insignias")]
    [Authorize]
    public class InsigniasController : ControllerBase
    {
        private readonly IMediator mediator;

        public InsigniasController(IMediator _mediator) => mediator = _mediator;

        [HttpGet]
        [Authorize(Policy = AppPermissions.Insignias.Read)]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var items = await mediator.Send(new GetInsigniasQuery(), ct);
            var response = items.Select(i => new InsigniaListItemResponse
            {
                Id = i.Id,
                Name = i.Name,
                Description = i.Description,
                ImageUrl = Request.ToAbsoluteUrl(i.ImageUrl),
                IsActive = i.IsActive,
                MinPoints = i.MinPoints,
                RequiredRegionCount = i.RequiredRegionCount,
                RequiredRetoCount = i.RequiredRetoCount,
                MinRetosCompleted = i.MinRetosCompleted,
                MinPerfectRetos = i.MinPerfectRetos,
                RequireAllQuestionTypes = i.RequireAllQuestionTypes,
                MinLevel = i.MinLevel,
                RequirePremium = i.RequirePremium,
                MinLoginStreakDays = i.MinLoginStreakDays,
                MinAnswerStreak = i.MinAnswerStreak,
                MinRegionsExplored = i.MinRegionsExplored,
                RequiredZoneCode = i.RequiredZoneCode,
                MinZoneRegionsExplored = i.MinZoneRegionsExplored,
                CreatedAt = i.CreatedAt
            }).ToList();
            return Ok(ApiResponse<List<InsigniaListItemResponse>>.Ok(response));
        }

        [HttpGet("{id:int}")]
        [Authorize(Policy = AppPermissions.Insignias.Read)]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var result = await mediator.Send(new GetInsigniaByIdQuery(id), ct);
            if (!result.IsSuccess) return this.FromResult(result);

            var d = result.Value!;
            return Ok(ApiResponse<InsigniaDetailResponse>.Ok(new InsigniaDetailResponse
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description,
                ImageUrl = Request.ToAbsoluteUrl(d.ImageUrl),
                IsActive = d.IsActive,
                MinPoints = d.MinPoints,
                RequiredRegions = d.RequiredRegions.Select(r => new RegionRequirementResponse
                {
                    RegionId = r.RegionId,
                    RegionTitle = r.RegionTitle
                }).ToList(),
                RequiredRetos = d.RequiredRetos.Select(r => new RetoRequirementResponse
                {
                    RetoId = r.RetoId,
                    CourseId = r.CourseId,
                    RetoTitle = r.RetoTitle,
                    RegionTitle = r.RegionTitle
                }).ToList(),
                MinRetosCompleted = d.MinRetosCompleted,
                MinPerfectRetos = d.MinPerfectRetos,
                RequireAllQuestionTypes = d.RequireAllQuestionTypes,
                MinLevel = d.MinLevel,
                RequirePremium = d.RequirePremium,
                MinLoginStreakDays = d.MinLoginStreakDays,
                MinAnswerStreak = d.MinAnswerStreak,
                MinRegionsExplored = d.MinRegionsExplored,
                RequiredZoneCode = d.RequiredZoneCode,
                MinZoneRegionsExplored = d.MinZoneRegionsExplored
            }));
        }

        [HttpPost]
        [Authorize(Policy = AppPermissions.Insignias.Create)]
        public async Task<IActionResult> Create([FromBody] UpsertInsigniaRequest request, CancellationToken ct)
        {
            var result = await mediator.Send(new CreateInsigniaCommand
            {
                Name = request.Name,
                Description = request.Description,
                IsActive = request.IsActive,
                MinPoints = request.MinPoints,
                RequiredRegionIds = request.RequiredRegionIds,
                RequiredRetoIds = request.RequiredRetoIds,
                MinRetosCompleted = request.MinRetosCompleted,
                MinPerfectRetos = request.MinPerfectRetos,
                RequireAllQuestionTypes = request.RequireAllQuestionTypes,
                MinLevel = request.MinLevel,
                RequirePremium = request.RequirePremium,
                MinLoginStreakDays = request.MinLoginStreakDays,
                MinAnswerStreak = request.MinAnswerStreak,
                MinRegionsExplored = request.MinRegionsExplored,
                RequiredZoneCode = request.RequiredZoneCode,
                MinZoneRegionsExplored = request.MinZoneRegionsExplored
            }, ct);
            return this.FromResult(result);
        }

        [HttpPut("{id:int}")]
        [Authorize(Policy = AppPermissions.Insignias.Update)]
        public async Task<IActionResult> Edit(int id, [FromBody] UpsertInsigniaRequest request, CancellationToken ct)
        {
            var result = await mediator.Send(new EditInsigniaCommand
            {
                Id = id,
                Name = request.Name,
                Description = request.Description,
                IsActive = request.IsActive,
                MinPoints = request.MinPoints,
                RequiredRegionIds = request.RequiredRegionIds,
                RequiredRetoIds = request.RequiredRetoIds,
                MinRetosCompleted = request.MinRetosCompleted,
                MinPerfectRetos = request.MinPerfectRetos,
                RequireAllQuestionTypes = request.RequireAllQuestionTypes,
                MinLevel = request.MinLevel,
                RequirePremium = request.RequirePremium,
                MinLoginStreakDays = request.MinLoginStreakDays,
                MinAnswerStreak = request.MinAnswerStreak,
                MinRegionsExplored = request.MinRegionsExplored,
                RequiredZoneCode = request.RequiredZoneCode,
                MinZoneRegionsExplored = request.MinZoneRegionsExplored
            }, ct);
            return this.FromResult(result);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy = AppPermissions.Insignias.Delete)]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var result = await mediator.Send(new DeleteInsigniaCommand(id), ct);
            return this.FromResult(result);
        }

        [HttpPost("{id:int}/image")]
        [Authorize(Policy = AppPermissions.Insignias.Update)]
        public async Task<IActionResult> UploadImage(int id, IFormFile file, CancellationToken ct)
        {
            if (file is null || file.Length == 0)
                return BadRequest(ApiResponse<string>.Fail("No se recibió ningún archivo."));

            await using var stream = file.OpenReadStream();
            var result = await mediator.Send(new UploadInsigniaImageCommand
            {
                Id = id,
                FileStream = stream,
                FileName = file.FileName
            }, ct);
            if (!result.IsSuccess) return this.FromResult(result);
            return Ok(ApiResponse<string>.Ok(Request.ToAbsoluteUrl(result.Value)));
        }
    }
}
