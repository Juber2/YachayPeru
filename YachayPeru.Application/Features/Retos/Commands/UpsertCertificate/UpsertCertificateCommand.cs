using MediatR;
using YachayPeru.Application.Common.Results;

namespace YachayPeru.Application.Features.Retos.Commands.UpsertCertificate
{
    public sealed record UpsertCertificateCommand : IRequest<Result>
    {
        public int RetoId { get; init; }
        public string MainTitle { get; init; } = default!;
        public string? Subtitle { get; init; }
        public string? BodyText { get; init; }
        public string? FooterText { get; init; }

        public CertificateInfoFieldsEntry IncludeFields { get; init; } = new();

        public string Orientation { get; init; } = default!;
        public string? Prefix { get; init; }

        public string PrimaryColor { get; init; } = default!;
        public string SecondaryColor { get; init; } = default!;
        public string AccentColor { get; init; } = default!;

        public string FontFamily { get; init; } = default!;
        public string BorderStyle { get; init; } = default!;
        public string BorderWidth { get; init; } = default!;

        public bool ShowLogo { get; init; }
        public string? SignerName { get; init; }
        public string? SignerTitle { get; init; }
        public bool ShowSeal { get; init; }
        public bool ShowWatermark { get; init; }
    }

    public class CertificateInfoFieldsEntry
    {
        public bool CompletionDate { get; set; }
        public bool Score { get; set; }
        public bool Duration { get; set; }
        public bool QrCode { get; set; }
        public bool EmployeeId { get; set; }
        public bool CertificateId { get; set; }
        public bool Instructor { get; set; }
        public bool Location { get; set; }
        public bool Modality { get; set; }
        public bool Validity { get; set; }
    }
}
