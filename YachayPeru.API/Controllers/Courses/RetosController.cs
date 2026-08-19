using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YachayPeru.API.Authorization;
using YachayPeru.API.Contracts.Common;
using YachayPeru.API.Contracts.Retos.Request;
using YachayPeru.API.Contracts.Retos.Response;
using YachayPeru.API.Extensions;
using YachayPeru.Application.Features.Retos.Commands.CreateReto;
using YachayPeru.Application.Features.Retos.Commands.CreateRetoDraft;
using YachayPeru.Application.Features.Retos.Commands.DeleteReto;
using YachayPeru.Application.Features.Retos.Commands.DiscardRetoDraft;
using YachayPeru.Application.Features.Retos.Commands.PublishReto;
using YachayPeru.Application.Features.Retos.Commands.Questions;
using YachayPeru.Application.Features.Retos.Commands.UpsertRetoSettings;
using YachayPeru.Application.Features.Retos.Queries.GetRetoById;
using YachayPeru.Application.Features.Retos.Queries.GetRetos;
using YachayPeru.Application.Features.Retos.Queries.GetRetoVersions;

namespace YachayPeru.API.Controllers.Courses
{
    [ApiController]
    [Route("courses/{courseId:int}/retos")]
    [Authorize]
    public class RetosController : ControllerBase
    {
        private readonly IMediator mediator;

        public RetosController(IMediator _mediator) => mediator = _mediator;

        [HttpGet]
        [Authorize(Policy = AppPermissions.CursosPlantilla.Read)]
        public async Task<IActionResult> GetAll(int courseId, CancellationToken ct)
        {
            var items = await mediator.Send(new GetRetosQuery(courseId), ct);
            var response = items.Select(r => new RetoListItemResponse
            {
                Id            = r.Id,
                Title         = r.Title,
                StatusCode    = r.StatusCode,
                QuestionCount = r.QuestionCount,
                TotalPoints   = r.TotalPoints,
                CreatedAt     = r.CreatedAt
            }).ToList();
            return Ok(ApiResponse<List<RetoListItemResponse>>.Ok(response));
        }

        [HttpPost]
        [Authorize(Policy = AppPermissions.CursosPlantilla.Update)]
        public async Task<IActionResult> Create(int courseId, CancellationToken ct)
        {
            var result = await mediator.Send(new CreateRetoCommand(courseId), ct);
            return this.FromResult(result);
        }

        [HttpDelete("{retoId:int}")]
        [Authorize(Policy = AppPermissions.CursosPlantilla.Update)]
        public async Task<IActionResult> Delete(int retoId, CancellationToken ct)
        {
            var result = await mediator.Send(new DeleteRetoCommand(retoId), ct);
            return this.FromResult(result);
        }

        [HttpGet("{retoId:int}")]
        [Authorize(Policy = AppPermissions.CursosPlantilla.Read)]
        public async Task<IActionResult> GetById(int retoId, CancellationToken ct)
        {
            var result = await mediator.Send(new GetRetoByIdQuery(retoId), ct);
            if (!result.IsSuccess) return this.FromResult(result);

            var d = result.Value!;
            return Ok(ApiResponse<RetoResponse>.Ok(new RetoResponse
            {
                Id                   = d.Id,
                VersionNumber        = d.VersionNumber,
                StatusCode           = d.StatusCode,
                Title                = d.Title,
                PassingScore         = d.PassingScore,
                TimeLimitMinutes     = d.TimeLimitMinutes,
                MaxAttempts          = d.MaxAttempts,
                ShuffleQuestionOrder = d.ShuffleQuestionOrder,
                ShuffleOptionOrder   = d.ShuffleOptionOrder,
                ShowResultsAtEnd     = d.ShowResultsAtEnd,
                Questions            = d.Questions.Select(q => new QuestionResponse
                {
                    Id               = q.Id,
                    QuestionTypeCode = q.QuestionTypeCode,
                    QuestionText     = q.QuestionText,
                    Points           = q.Points,
                    OrderIndex       = q.OrderIndex,
                    Choices          = q.Choices.Select(c => new ChoiceResponse
                    {
                        Id         = c.Id,
                        Text       = c.Text,
                        IsCorrect  = c.IsCorrect,
                        OrderIndex = c.OrderIndex
                    }).ToList()
                }).ToList()
            }));
        }

        [HttpPut("{retoId:int}/settings")]
        [Authorize(Policy = AppPermissions.CursosPlantilla.Update)]
        public async Task<IActionResult> UpsertSettings(int retoId, [FromBody] UpsertRetoSettingsRequest request, CancellationToken ct)
        {
            var result = await mediator.Send(new UpsertRetoSettingsCommand
            {
                RetoId               = retoId,
                Title                = request.Title,
                PassingScore         = request.PassingScore,
                TimeLimitMinutes     = request.TimeLimitMinutes,
                MaxAttempts          = request.MaxAttempts,
                ShuffleQuestionOrder = request.ShuffleQuestionOrder,
                ShuffleOptionOrder   = request.ShuffleOptionOrder,
                ShowResultsAtEnd     = request.ShowResultsAtEnd
            }, ct);
            return this.FromResult(result);
        }

        [HttpPost("{retoId:int}/questions")]
        [Authorize(Policy = AppPermissions.CursosPlantilla.Update)]
        public async Task<IActionResult> AddQuestion(int retoId, [FromBody] AddRetoQuestionRequest request, CancellationToken ct)
        {
            var result = await mediator.Send(new AddRetoQuestionCommand
            {
                RetoId           = retoId,
                QuestionTypeCode = request.QuestionTypeCode,
                QuestionText     = request.QuestionText,
                Points           = request.Points,
                Choices          = request.Choices.Select(c => new QuestionChoiceEntry { Text = c.Text, IsCorrect = c.IsCorrect, OrderIndex = c.OrderIndex }).ToList(),
                Blanks           = request.Blanks.Select(b => new QuestionBlankEntry { BlankIndex = b.BlankIndex, CorrectAnswer = b.CorrectAnswer, OrderIndex = b.OrderIndex }).ToList()
            }, ct);
            return this.FromResult(result);
        }

        [HttpPut("{retoId:int}/questions/{questionId:int}")]
        [Authorize(Policy = AppPermissions.CursosPlantilla.Update)]
        public async Task<IActionResult> EditQuestion(int questionId, [FromBody] EditRetoQuestionRequest request, CancellationToken ct)
        {
            var result = await mediator.Send(new EditRetoQuestionCommand
            {
                QuestionId   = questionId,
                QuestionText = request.QuestionText,
                Points       = request.Points,
                Choices      = request.Choices.Select(c => new QuestionChoiceEntry { Text = c.Text, IsCorrect = c.IsCorrect, OrderIndex = c.OrderIndex }).ToList(),
                Blanks       = request.Blanks.Select(b => new QuestionBlankEntry { BlankIndex = b.BlankIndex, CorrectAnswer = b.CorrectAnswer, OrderIndex = b.OrderIndex }).ToList()
            }, ct);
            return this.FromResult(result);
        }

        [HttpDelete("{retoId:int}/questions/{questionId:int}")]
        [Authorize(Policy = AppPermissions.CursosPlantilla.Update)]
        public async Task<IActionResult> DeleteQuestion(int questionId, CancellationToken ct)
        {
            var result = await mediator.Send(new DeleteRetoQuestionCommand(questionId), ct);
            return this.FromResult(result);
        }

        [HttpPut("{retoId:int}/questions/reorder")]
        [Authorize(Policy = AppPermissions.CursosPlantilla.Update)]
        public async Task<IActionResult> ReorderQuestions(int retoId, [FromBody] ReorderQuestionsRequest request, CancellationToken ct)
        {
            var result = await mediator.Send(new ReorderRetoQuestionsCommand
            {
                RetoId        = retoId,
                Items         = request.Items.Select(i => new QuestionOrderEntry { QuestionId = i.QuestionId, OrderIndex = i.OrderIndex }).ToList()
            }, ct);
            return this.FromResult(result);
        }

        [HttpPost("{retoId:int}/publish")]
        [Authorize(Policy = AppPermissions.CursosPlantilla.Update)]
        public async Task<IActionResult> Publish(int retoId, CancellationToken ct)
        {
            var result = await mediator.Send(new PublishRetoCommand(retoId), ct);
            return this.FromResult(result);
        }

        [HttpPost("{retoId:int}/draft")]
        [Authorize(Policy = AppPermissions.CursosPlantilla.Update)]
        public async Task<IActionResult> CreateDraft(int retoId, CancellationToken ct)
        {
            var result = await mediator.Send(new CreateRetoDraftCommand(retoId), ct);
            return this.FromResult(result);
        }

        [HttpDelete("{retoId:int}/draft")]
        [Authorize(Policy = AppPermissions.CursosPlantilla.Update)]
        public async Task<IActionResult> DiscardDraft(int retoId, CancellationToken ct)
        {
            var result = await mediator.Send(new DiscardRetoDraftCommand(retoId), ct);
            return this.FromResult(result);
        }

        [HttpGet("{retoId:int}/versions")]
        [Authorize(Policy = AppPermissions.CursosPlantilla.Read)]
        public async Task<IActionResult> GetVersions(int retoId, CancellationToken ct)
        {
            var items = await mediator.Send(new GetRetoVersionsQuery(retoId), ct);
            var response = items.Select(v => new RetoVersionSummaryResponse
            {
                Id            = v.Id,
                VersionNumber = v.VersionNumber,
                StatusCode    = v.StatusCode,
                Title         = v.Title,
                CreatedAt     = v.CreatedAt
            }).ToList();
            return Ok(ApiResponse<List<RetoVersionSummaryResponse>>.Ok(response));
        }

        [HttpGet("{retoId:int}/versions/{versionId:int}")]
        [Authorize(Policy = AppPermissions.CursosPlantilla.Read)]
        public async Task<IActionResult> GetVersionDetail(int retoId, int versionId, CancellationToken ct)
        {
            var result = await mediator.Send(new GetRetoVersionDetailQuery(versionId, retoId), ct);
            if (!result.IsSuccess) return this.FromResult(result);

            var d = result.Value!;
            return Ok(ApiResponse<RetoVersionDetailResponse>.Ok(new RetoVersionDetailResponse
            {
                Id                   = d.Id,
                VersionNumber        = d.VersionNumber,
                StatusCode           = d.StatusCode,
                Title                = d.Title,
                PassingScore         = d.PassingScore,
                TimeLimitMinutes     = d.TimeLimitMinutes,
                MaxAttempts          = d.MaxAttempts,
                ShuffleQuestionOrder = d.ShuffleQuestionOrder,
                ShuffleOptionOrder   = d.ShuffleOptionOrder,
                ShowResultsAtEnd     = d.ShowResultsAtEnd,
                CreatedAt            = d.CreatedAt,
                Questions            = d.Questions.Select(q => new QuestionResponse
                {
                    Id               = q.Id,
                    QuestionTypeCode = q.QuestionTypeCode,
                    QuestionText     = q.QuestionText,
                    Points           = q.Points,
                    OrderIndex       = q.OrderIndex,
                    Choices          = q.Choices.Select(c => new ChoiceResponse
                    {
                        Id         = c.Id,
                        Text       = c.Text,
                        IsCorrect  = c.IsCorrect,
                        OrderIndex = c.OrderIndex
                    }).ToList()
                }).ToList()
            }));
        }
    }
}
