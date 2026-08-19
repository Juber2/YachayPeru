namespace YachayPeru.Application.Common.Settings
{
    public sealed class SecuritySettings
    {
        public const string SectionName = "SecuritySettings";

        /// <summary>URL base de la API — usada para construir los links del email de aprobación.</summary>
        public string BaseUrl { get; set; } = "https://localhost:5001";

        /// <summary>Activa la detección de IP sospechosa al hacer refresh.</summary>
        public bool EnableSuspiciousIpDetection { get; set; } = true;

        /// <summary>Horas de validez del link de aprobación enviado por email.</summary>
        public int ApprovalTokenExpirationHours { get; set; } = 24;
    }
}
