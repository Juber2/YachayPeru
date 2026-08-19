namespace YachayPeru.Application.Actions.Courses.Models
{
    public class UpsertCertificateInput
    {
        public int RetoId { get; set; }
        public string MainTitle { get; set; } = string.Empty;
        public string? Subtitle { get; set; }
        public string? BodyText { get; set; }
        public string? FooterText { get; set; }

        public CertificateInfoFieldsInput IncludeFields { get; set; } = new();

        public string Orientation { get; set; } = string.Empty;
        public string? Prefix { get; set; }

        public string PrimaryColor { get; set; } = string.Empty;
        public string SecondaryColor { get; set; } = string.Empty;
        public string AccentColor { get; set; } = string.Empty;

        public string FontFamily { get; set; } = string.Empty;
        public string BorderStyle { get; set; } = string.Empty;
        public string BorderWidth { get; set; } = string.Empty;

        public bool ShowLogo { get; set; }
        public string? SignerName { get; set; }
        public string? SignerTitle { get; set; }
        public bool ShowSeal { get; set; }
        public bool ShowWatermark { get; set; }
    }

    public class CertificateInfoFieldsInput
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
