namespace YachayPeru.Application.Abstractions.Services
{
    public class CertificatePdfData
    {
        public string MainTitle { get; set; } = string.Empty;
        public string? Subtitle { get; set; }
        public string? BodyText { get; set; }
        public string? FooterText { get; set; }

        public bool IncludeCompletionDate { get; set; }
        public bool IncludeScore { get; set; }
        public bool IncludeDuration { get; set; }
        public bool IncludeQrCode { get; set; }
        public bool IncludeEmployeeId { get; set; }
        public bool IncludeCertificateId { get; set; }
        public bool IncludeInstructor { get; set; }
        public bool IncludeLocation { get; set; }
        public bool IncludeModality { get; set; }
        public bool IncludeValidity { get; set; }

        public string Orientation { get; set; } = "horizontal";
        public string? Prefix { get; set; }

        public string PrimaryColor { get; set; } = "#4F46E5";
        public string SecondaryColor { get; set; } = "#4F46E5";
        public string AccentColor { get; set; } = "#4F46E5";

        public string FontFamily { get; set; } = "sans";
        public string BorderStyle { get; set; } = "simple";
        public string BorderWidth { get; set; } = "medio";

        public bool ShowLogo { get; set; }
        public byte[]? LogoBytes { get; set; }

        public string? SignerName { get; set; }
        public string? SignerTitle { get; set; }
        public byte[]? SignatureBytes { get; set; }

        public bool ShowSeal { get; set; }
        public byte[]? SealBytes { get; set; }

        public bool ShowWatermark { get; set; }

        // Datos dinámicos calculados por el backend
        public string StudentName { get; set; } = string.Empty;
        public string RetoTitle { get; set; } = string.Empty;
        public string RegionTitle { get; set; } = string.Empty;
        public DateTime CompletionDate { get; set; }
        public decimal Score { get; set; }
        public string CertificateId { get; set; } = string.Empty;
        public string? QrContent { get; set; }
    }

    public interface ICertificatePdfRenderer
    {
        byte[] Render(CertificatePdfData data);
    }
}
