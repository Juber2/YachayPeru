using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YachayPeru.API.Authorization;
using YachayPeru.API.Contracts.Common;
using YachayPeru.API.Contracts.Retos.Request;
using YachayPeru.API.Contracts.Retos.Response;
using YachayPeru.API.Extensions;
using YachayPeru.Application.Features.Retos.Commands.UploadCertificateLogo;
using YachayPeru.Application.Features.Retos.Commands.UploadCertificateSeal;
using YachayPeru.Application.Features.Retos.Commands.UploadCertificateSignature;
using YachayPeru.Application.Features.Retos.Commands.UpsertCertificate;
using YachayPeru.Application.Features.Retos.Queries.GetCertificate;
using YachayPeru.Application.Features.Retos.Queries.GetCertificateList;

namespace YachayPeru.API.Controllers.Courses
{
    [ApiController]
    [Route("courses/{courseId:int}/retos")]
    [Authorize]
    public class RetoCertificatesController : ControllerBase
    {
        private readonly IMediator mediator;

        public RetoCertificatesController(IMediator _mediator) => mediator = _mediator;

        [HttpGet("certificates")]
        [Authorize(Policy = AppPermissions.CursosPlantilla.Read)]
        public async Task<IActionResult> GetList(int courseId, CancellationToken ct)
        {
            var items = await mediator.Send(new GetCertificateListQuery(courseId), ct);
            var response = items.Select(i => new CertificateListItemResponse
            {
                RetoId       = i.RetoId,
                RetoTitle    = i.RetoTitle,
                IsConfigured = i.IsConfigured
            }).ToList();
            return Ok(ApiResponse<List<CertificateListItemResponse>>.Ok(response));
        }

        [HttpGet("{retoId:int}/certificate")]
        [Authorize(Policy = AppPermissions.CursosPlantilla.Read)]
        public async Task<IActionResult> GetCertificate(int retoId, CancellationToken ct)
        {
            var d = await mediator.Send(new GetCertificateQuery(retoId), ct);
            if (d is null)
                return Ok(ApiResponse<CertificateResponse?>.Ok(null));

            return Ok(ApiResponse<CertificateResponse?>.Ok(new CertificateResponse
            {
                RetoId = d.RetoId,
                MainTitle = d.MainTitle,
                Subtitle = d.Subtitle,
                BodyText = d.BodyText,
                FooterText = d.FooterText,
                IncludeFields = new CertificateInfoFieldsResponse
                {
                    CompletionDate = d.IncludeFields.CompletionDate,
                    Score = d.IncludeFields.Score,
                    Duration = d.IncludeFields.Duration,
                    QrCode = d.IncludeFields.QrCode,
                    EmployeeId = d.IncludeFields.EmployeeId,
                    CertificateId = d.IncludeFields.CertificateId,
                    Instructor = d.IncludeFields.Instructor,
                    Location = d.IncludeFields.Location,
                    Modality = d.IncludeFields.Modality,
                    Validity = d.IncludeFields.Validity
                },
                Orientation = d.Orientation,
                Prefix = d.Prefix,
                PrimaryColor = d.PrimaryColor,
                SecondaryColor = d.SecondaryColor,
                AccentColor = d.AccentColor,
                FontFamily = d.FontFamily,
                BorderStyle = d.BorderStyle,
                BorderWidth = d.BorderWidth,
                ShowLogo = d.ShowLogo,
                LogoUrl = Request.ToAbsoluteUrl(d.LogoUrl),
                SignerName = d.SignerName,
                SignerTitle = d.SignerTitle,
                SignatureUrl = Request.ToAbsoluteUrl(d.SignatureUrl),
                ShowSeal = d.ShowSeal,
                SealUrl = Request.ToAbsoluteUrl(d.SealUrl),
                ShowWatermark = d.ShowWatermark
            }));
        }

        [HttpPut("{retoId:int}/certificate")]
        [Authorize(Policy = AppPermissions.CursosPlantilla.Update)]
        public async Task<IActionResult> UpsertCertificate(int retoId, [FromBody] UpsertCertificateRequest request, CancellationToken ct)
        {
            var result = await mediator.Send(new UpsertCertificateCommand
            {
                RetoId = retoId,
                MainTitle = request.MainTitle,
                Subtitle = request.Subtitle,
                BodyText = request.BodyText,
                FooterText = request.FooterText,
                IncludeFields = new CertificateInfoFieldsEntry
                {
                    CompletionDate = request.IncludeFields.CompletionDate,
                    Score = request.IncludeFields.Score,
                    Duration = request.IncludeFields.Duration,
                    QrCode = request.IncludeFields.QrCode,
                    EmployeeId = request.IncludeFields.EmployeeId,
                    CertificateId = request.IncludeFields.CertificateId,
                    Instructor = request.IncludeFields.Instructor,
                    Location = request.IncludeFields.Location,
                    Modality = request.IncludeFields.Modality,
                    Validity = request.IncludeFields.Validity
                },
                Orientation = request.Orientation,
                Prefix = request.Prefix,
                PrimaryColor = request.PrimaryColor,
                SecondaryColor = request.SecondaryColor,
                AccentColor = request.AccentColor,
                FontFamily = request.FontFamily,
                BorderStyle = request.BorderStyle,
                BorderWidth = request.BorderWidth,
                ShowLogo = request.ShowLogo,
                SignerName = request.SignerName,
                SignerTitle = request.SignerTitle,
                ShowSeal = request.ShowSeal,
                ShowWatermark = request.ShowWatermark
            }, ct);
            return this.FromResult(result);
        }

        [HttpPost("{retoId:int}/certificate/logo")]
        [Authorize(Policy = AppPermissions.CursosPlantilla.Update)]
        public async Task<IActionResult> UploadLogo(int retoId, IFormFile file, CancellationToken ct)
        {
            if (file is null || file.Length == 0)
                return BadRequest(ApiResponse<string>.Fail("No se recibió ningún archivo."));

            await using var stream = file.OpenReadStream();
            var result = await mediator.Send(new UploadCertificateLogoCommand
            {
                RetoId = retoId,
                FileStream = stream,
                FileName = file.FileName
            }, ct);
            if (!result.IsSuccess) return this.FromResult(result);
            return Ok(ApiResponse<string>.Ok(Request.ToAbsoluteUrl(result.Value)));
        }

        [HttpPost("{retoId:int}/certificate/signature")]
        [Authorize(Policy = AppPermissions.CursosPlantilla.Update)]
        public async Task<IActionResult> UploadSignature(int retoId, IFormFile file, CancellationToken ct)
        {
            if (file is null || file.Length == 0)
                return BadRequest(ApiResponse<string>.Fail("No se recibió ningún archivo."));

            await using var stream = file.OpenReadStream();
            var result = await mediator.Send(new UploadCertificateSignatureCommand
            {
                RetoId = retoId,
                FileStream = stream,
                FileName = file.FileName
            }, ct);
            if (!result.IsSuccess) return this.FromResult(result);
            return Ok(ApiResponse<string>.Ok(Request.ToAbsoluteUrl(result.Value)));
        }

        [HttpPost("{retoId:int}/certificate/seal")]
        [Authorize(Policy = AppPermissions.CursosPlantilla.Update)]
        public async Task<IActionResult> UploadSeal(int retoId, IFormFile file, CancellationToken ct)
        {
            if (file is null || file.Length == 0)
                return BadRequest(ApiResponse<string>.Fail("No se recibió ningún archivo."));

            await using var stream = file.OpenReadStream();
            var result = await mediator.Send(new UploadCertificateSealCommand
            {
                RetoId = retoId,
                FileStream = stream,
                FileName = file.FileName
            }, ct);
            if (!result.IsSuccess) return this.FromResult(result);
            return Ok(ApiResponse<string>.Ok(Request.ToAbsoluteUrl(result.Value)));
        }
    }
}
