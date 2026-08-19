namespace YachayPeru.API.Contracts.Retos.Response
{
    public record CertificateListItemResponse
    {
        public int RetoId { get; init; }
        public string RetoTitle { get; init; } = string.Empty;
        public bool IsConfigured { get; init; }
    }

    public record CertificateResponse
    {
        public int RetoId { get; init; }
        public string MainTitle { get; init; } = string.Empty;
        public string? Subtitle { get; init; }
        public string? BodyText { get; init; }
        public string? FooterText { get; init; }
        public CertificateInfoFieldsResponse IncludeFields { get; init; } = new();
        public string Orientation { get; init; } = string.Empty;
        public string? Prefix { get; init; }
        public string PrimaryColor { get; init; } = string.Empty;
        public string SecondaryColor { get; init; } = string.Empty;
        public string AccentColor { get; init; } = string.Empty;
        public string FontFamily { get; init; } = string.Empty;
        public string BorderStyle { get; init; } = string.Empty;
        public string BorderWidth { get; init; } = string.Empty;
        public bool ShowLogo { get; init; }
        public string? LogoUrl { get; init; }
        public string? SignerName { get; init; }
        public string? SignerTitle { get; init; }
        public string? SignatureUrl { get; init; }
        public bool ShowSeal { get; init; }
        public string? SealUrl { get; init; }
        public bool ShowWatermark { get; init; }
    }

    public record CertificateInfoFieldsResponse
    {
        public bool CompletionDate { get; init; }
        public bool Score { get; init; }
        public bool Duration { get; init; }
        public bool QrCode { get; init; }
        public bool EmployeeId { get; init; }
        public bool CertificateId { get; init; }
        public bool Instructor { get; init; }
        public bool Location { get; init; }
        public bool Modality { get; init; }
        public bool Validity { get; init; }
    }
}
