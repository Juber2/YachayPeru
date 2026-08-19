namespace YachayPeru.Domain.Entities.Auth
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; }

        // Rotation: token anterior que fue reemplazado por este
        public string? ReplacedByToken { get; set; }

        // Contexto del login original
        public string? LoginIp { get; set; }
        public string? LoginUserAgent { get; set; }

        // Aprobación de sesión sospechosa
        public bool IsPendingApproval { get; set; }
        public string? ApprovalToken { get; set; }
        public DateTime? ApprovalExpiresAt { get; set; }
        public string? SuspiciousIp { get; set; }

        public DateTime CreatedAt { get; set; }
        public User User { get; set; } = null!;
    }
}
