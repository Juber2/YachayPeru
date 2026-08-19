using YachayPeru.Domain.Base;

namespace YachayPeru.Domain.Entities.Learning
{
    public class CertificateTemplate : BaseEntity
    {
        public int RetoId { get; set; }
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

        public string Orientation { get; set; } = string.Empty;
        public string? Prefix { get; set; }

        public string PrimaryColor { get; set; } = string.Empty;
        public string SecondaryColor { get; set; } = string.Empty;
        public string AccentColor { get; set; } = string.Empty;

        public string FontFamily { get; set; } = string.Empty;
        public string BorderStyle { get; set; } = string.Empty;
        public string BorderWidth { get; set; } = string.Empty;

        public bool ShowLogo { get; set; }
        public string? LogoUrl { get; set; }

        public string? SignerName { get; set; }
        public string? SignerTitle { get; set; }
        public string? SignatureUrl { get; set; }

        public bool ShowSeal { get; set; }
        public string? SealUrl { get; set; }

        public bool ShowWatermark { get; set; }
    }
}
