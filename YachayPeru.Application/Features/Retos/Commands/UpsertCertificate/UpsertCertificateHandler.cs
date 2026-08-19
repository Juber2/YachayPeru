using MediatR;
using YachayPeru.Application.Actions.Courses;
using YachayPeru.Application.Actions.Courses.Models;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Retos.Commands.UpsertCertificate
{
    public class UpsertCertificateHandler : IRequestHandler<UpsertCertificateCommand, Result>
    {
        private readonly CertificateActions certificateActions;
        public UpsertCertificateHandler(CertificateActions _certificateActions) => certificateActions = _certificateActions;

        public Task<Result> Handle(UpsertCertificateCommand request, CancellationToken ct)
            => certificateActions.UpsertCertificate(new UpsertCertificateInput
            {
                RetoId = request.RetoId,
                MainTitle = request.MainTitle,
                Subtitle = request.Subtitle,
                BodyText = request.BodyText,
                FooterText = request.FooterText,
                IncludeFields = new CertificateInfoFieldsInput
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
    }
}
