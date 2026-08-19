using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YachayPeru.API.Authorization;
using YachayPeru.API.Contracts.Administration.Premium.Request;
using YachayPeru.API.Contracts.Administration.Premium.Response;
using YachayPeru.API.Contracts.Common;
using YachayPeru.API.Extensions;
using YachayPeru.Application.Features.Administration.Premium.Commands.ReviewWaitlist;
using YachayPeru.Application.Features.Administration.Premium.Queries.GetWaitlist;

namespace YachayPeru.API.Controllers.Administration
{
    [ApiController]
    [Route("administration/premium")]
    [Authorize]
    public class PremiumController : ControllerBase
    {
        private readonly IMediator mediator;

        public PremiumController(IMediator _mediator) => mediator = _mediator;

        [HttpGet("waitlist")]
        [Authorize(Policy = AppPermissions.Premium.Read)]
        public async Task<IActionResult> GetWaitlist(CancellationToken ct)
        {
            var result = await mediator.Send(new GetWaitlistQuery(), ct);
            if (!result.IsSuccess) return this.FromResult(result);

            var response = result.Value!.Select(e => new PremiumWaitlistEntryResponse
            {
                Id = e.Id,
                UserId = e.UserId,
                UserFullName = e.UserFullName,
                UserEmail = e.UserEmail,
                PlanId = e.PlanId,
                PlanName = e.PlanName,
                PaymentMethod = e.PaymentMethod,
                ReceiptUrl = Request.ToAbsoluteUrl(e.ReceiptUrl),
                Status = e.Status,
                RejectionReason = e.RejectionReason,
                JoinedAt = e.JoinedAt
            }).ToList();

            return Ok(ApiResponse<List<PremiumWaitlistEntryResponse>>.Ok(response));
        }

        [HttpPut("waitlist/{userId:int}/estado")]
        [Authorize(Policy = AppPermissions.Premium.Update)]
        public async Task<IActionResult> ReviewWaitlist(int userId, [FromBody] ReviewWaitlistRequest request, CancellationToken ct)
        {
            var result = await mediator.Send(new ReviewWaitlistCommand(userId, request.Status, request.RejectionReason), ct);
            return this.FromResult(result);
        }
    }
}
