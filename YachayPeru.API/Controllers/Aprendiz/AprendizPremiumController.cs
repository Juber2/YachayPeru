using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YachayPeru.API.Contracts.Aprendiz.Premium.Request;
using YachayPeru.API.Contracts.Aprendiz.Premium.Response;
using YachayPeru.API.Contracts.Common;
using YachayPeru.API.Authorization;
using YachayPeru.API.Extensions;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Application.Features.Aprendiz.Premium.Commands.MarkReviewSeen;
using YachayPeru.Application.Features.Aprendiz.Premium.Commands.PostWaitlist;
using YachayPeru.Application.Features.Aprendiz.Premium.Commands.UploadReceipt;
using YachayPeru.Application.Features.Aprendiz.Premium.Queries.GetPlans;

namespace YachayPeru.API.Controllers.Aprendiz
{
    [ApiController]
    [Route("aprendiz/premium")]
    [Authorize(Policy = AppPermissions.AprendizPremium.Read)]
    public class AprendizPremiumController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ICurrentUser currentUser;

        public AprendizPremiumController(IMediator _mediator, ICurrentUser _currentUser)
        {
            mediator = _mediator;
            currentUser = _currentUser;
        }

        [HttpGet("planes")]
        public async Task<IActionResult> GetPlans(CancellationToken ct)
        {
            var result = await mediator.Send(new GetPlansQuery(currentUser.Id), ct);
            return Ok(ApiResponse<AprendizPremiumPlansResponse>.Ok(new AprendizPremiumPlansResponse
            {
                IsPremiumUser = result.IsPremiumUser,
                SelectedPlanId = result.SelectedPlanId,
                WaitlistStatus = result.WaitlistStatus,
                RejectionReason = result.RejectionReason,
                HasUnseenReview = result.HasUnseenReview,
                Plans = result.Plans.Select(p => new AprendizPremiumPlanCardResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Features = p.Features.ToList()
                }).ToList()
            }));
        }

        [HttpPost("waitlist")]
        public async Task<IActionResult> PostWaitlist([FromBody] PostWaitlistRequest request, CancellationToken ct)
        {
            var result = await mediator.Send(new PostWaitlistCommand(currentUser.Id, request.PlanId, request.PaymentMethod), ct);
            return this.FromResult(result);
        }

        [HttpPost("waitlist/comprobante")]
        public async Task<IActionResult> UploadReceipt(IFormFile file, CancellationToken ct)
        {
            if (file is null || file.Length == 0)
                return BadRequest(ApiResponse<string>.Fail("No se recibió ningún archivo."));

            await using var stream = file.OpenReadStream();
            var result = await mediator.Send(new UploadReceiptCommand
            {
                UserId = currentUser.Id,
                FileStream = stream,
                FileName = file.FileName
            }, ct);
            if (!result.IsSuccess) return this.FromResult(result);

            return Ok(ApiResponse<string>.Ok(Request.ToAbsoluteUrl(result.Value)));
        }

        [HttpPut("waitlist/visto")]
        public async Task<IActionResult> MarkReviewSeen(CancellationToken ct)
        {
            var result = await mediator.Send(new MarkReviewSeenCommand(currentUser.Id), ct);
            return this.FromResult(result);
        }
    }
}
