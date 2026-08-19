using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YachayPeru.API.Contracts.Aprendiz.Retos.Request;
using YachayPeru.API.Contracts.Aprendiz.Retos.Response;
using YachayPeru.API.Contracts.Common;
using YachayPeru.API.Authorization;
using YachayPeru.API.Extensions;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Application.Features.Aprendiz.Retos.Commands.PostIntento;
using YachayPeru.Application.Features.Aprendiz.Retos.Queries.GetRetoById;
using YachayPeru.Application.Features.Aprendiz.Retos.Queries.GetRetos;

namespace YachayPeru.API.Controllers.Aprendiz
{
    [ApiController]
    [Route("aprendiz/retos")]
    [Authorize(Policy = AppPermissions.AprendizRetos.Read)]
    public class AprendizRetosController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ICurrentUser currentUser;

        public AprendizRetosController(IMediator _mediator, ICurrentUser _currentUser)
        {
            mediator = _mediator;
            currentUser = _currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int? regionId, CancellationToken ct)
        {
            var items = await mediator.Send(new GetRetosQuery(currentUser.Id, regionId), ct);
            var response = items.Select(r => new AprendizRetoListItemResponse
            {
                Id = r.Id,
                Title = r.Title,
                RegionId = r.RegionId,
                RegionTitle = r.RegionTitle,
                QuestionCount = r.QuestionCount,
                TotalPoints = r.TotalPoints,
                EarnedPoints = r.EarnedPoints,
                IsCompleted = r.IsCompleted,
                AttemptsUsed = r.AttemptsUsed,
                MaxAttempts = r.MaxAttempts
            }).ToList();
            return Ok(ApiResponse<List<AprendizRetoListItemResponse>>.Ok(response));
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var result = await mediator.Send(new GetRetoByIdQuery(id), ct);
            if (!result.IsSuccess) return this.FromResult(result);

            var d = result.Value!;
            return Ok(ApiResponse<AprendizRetoPlayResponse>.Ok(new AprendizRetoPlayResponse
            {
                Id = d.Id,
                Title = d.Title,
                TimeLimitMinutes = d.TimeLimitMinutes,
                PassingScore = d.PassingScore,
                ShuffleQuestionOrder = d.ShuffleQuestionOrder,
                ShuffleOptionOrder = d.ShuffleOptionOrder,
                Questions = d.Questions.Select(q => new AprendizQuestionResponse
                {
                    Id = q.Id,
                    QuestionTypeCode = q.QuestionTypeCode,
                    QuestionText = q.QuestionText,
                    Points = q.Points,
                    OrderIndex = q.OrderIndex,
                    Choices = q.Choices.Select(c => new AprendizChoiceResponse
                    {
                        Id = c.Id,
                        Text = c.Text,
                        OrderIndex = c.OrderIndex
                    }).ToList(),
                    BlanksCount = q.BlanksCount
                }).ToList()
            }));
        }

        [HttpPost("{id:int}/intentos")]
        public async Task<IActionResult> PostIntento(int id, [FromBody] RetoAttemptRequest request, CancellationToken ct)
        {
            var result = await mediator.Send(new PostIntentoCommand
            {
                UserId = currentUser.Id,
                RetoId = id,
                Answers = request.Answers.Select(a => new AnswerEntry
                {
                    QuestionId = a.QuestionId,
                    SelectedChoiceIds = a.SelectedChoiceIds,
                    BlankAnswers = a.BlankAnswers
                }).ToList()
            }, ct);

            if (!result.IsSuccess) return this.FromResult(result);

            var d = result.Value!;
            return Ok(ApiResponse<RetoAttemptResultResponse>.Ok(new RetoAttemptResultResponse
            {
                EarnedPoints = d.EarnedPoints,
                TotalPoints = d.TotalPoints,
                Passed = d.Passed,
                CorrectCount = d.CorrectCount,
                TotalQuestions = d.TotalQuestions,
                PerQuestion = d.PerQuestion.Select(p => new PerQuestionResultResponse
                {
                    QuestionId = p.QuestionId,
                    IsCorrect = p.IsCorrect,
                    CorrectChoiceIds = p.CorrectChoiceIds,
                    CorrectBlankAnswers = p.CorrectBlankAnswers
                }).ToList()
            }));
        }
    }
}
