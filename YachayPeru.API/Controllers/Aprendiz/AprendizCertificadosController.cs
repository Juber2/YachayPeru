using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YachayPeru.API.Contracts.Aprendiz.Certificados.Response;
using YachayPeru.API.Contracts.Common;
using YachayPeru.API.Authorization;
using YachayPeru.API.Extensions;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Application.Features.Aprendiz.Certificados.Queries.GetCertificadoPdf;
using YachayPeru.Application.Features.Aprendiz.Certificados.Queries.GetCertificados;

namespace YachayPeru.API.Controllers.Aprendiz
{
    [ApiController]
    [Route("aprendiz/certificados")]
    [Authorize(Policy = AppPermissions.AprendizCertificados.Read)]
    public class AprendizCertificadosController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly ICurrentUser currentUser;

        public AprendizCertificadosController(IMediator _mediator, ICurrentUser _currentUser)
        {
            mediator = _mediator;
            currentUser = _currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var items = await mediator.Send(new GetCertificadosQuery(currentUser.Id), ct);
            var response = items.Select(i => new AprendizCertificadoListItemResponse
            {
                RetoId = i.RetoId,
                RetoTitle = i.RetoTitle,
                IsAvailable = i.IsAvailable,
                DownloadUrl = i.IsAvailable
                    ? $"{Request.Scheme}://{Request.Host}/api/v1/aprendiz/certificados/{i.RetoId}/pdf"
                    : null
            }).ToList();
            return Ok(ApiResponse<List<AprendizCertificadoListItemResponse>>.Ok(response));
        }

        [HttpGet("{retoId:int}/pdf")]
        public async Task<IActionResult> GetPdf(int retoId, CancellationToken ct)
        {
            var result = await mediator.Send(new GetCertificadoPdfQuery(currentUser.Id, retoId), ct);
            if (!result.IsSuccess) return this.FromResult(result);

            return File(result.Value!, "application/pdf", $"certificado-{retoId}.pdf");
        }
    }
}
