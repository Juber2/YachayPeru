namespace YachayPeru.Application.Abstractions.Services
{
    public interface IEmailService
    {
        Task SendSuspiciousSessionAlertAsync(
            string toEmail,
            string userName,
            string suspiciousIp,
            string approveUrl,
            string revokeUrl,
            CancellationToken ct = default);

        Task SendWelcomeEmailAsync(
            string toEmail,
            string fullName,
            string username,
            string temporaryPassword,
            CancellationToken ct = default);

        Task SendPremiumReviewEmailAsync(
            string toEmail,
            string fullName,
            string planName,
            bool approved,
            string? rejectionReason,
            CancellationToken ct = default);
    }
}
