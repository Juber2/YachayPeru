namespace YachayPeru.API.Contracts.Retos.Request
{
    public class UpsertCertificateRequest
    {
        public string MainTitle { get; set; } = default!;
        public string? Subtitle { get; set; }
        public string? BodyText { get; set; }
        public string? FooterText { get; set; }
        public CertificateInfoFieldsRequest IncludeFields { get; set; } = new();
        public string Orientation { get; set; } = default!;
        public string? Prefix { get; set; }
        public string PrimaryColor { get; set; } = default!;
        public string SecondaryColor { get; set; } = default!;
        public string AccentColor { get; set; } = default!;
        public string FontFamily { get; set; } = default!;
        public string BorderStyle { get; set; } = default!;
        public string BorderWidth { get; set; } = default!;
        public bool ShowLogo { get; set; }
        public string? SignerName { get; set; }
        public string? SignerTitle { get; set; }
        public bool ShowSeal { get; set; }
        public bool ShowWatermark { get; set; }
    }

    public class CertificateInfoFieldsRequest
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
