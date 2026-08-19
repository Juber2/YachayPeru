using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Application.Common.Settings;

namespace YachayPeru.Infrastructure.Services
{
    /// <summary>
    /// Implementación de desarrollo: loguea los emails en lugar de enviarlos.
    /// Reemplazar por SendGrid / SMTP en producción.
    /// </summary>
    public sealed class LogEmailService //: IEmailService
    {
        private readonly ILogger<LogEmailService> _logger;
        private readonly WelcomeEmailSettings _welcomeSettings;

        public LogEmailService(ILogger<LogEmailService> logger, IOptions<WelcomeEmailSettings> welcomeOptions)
        {
            _logger = logger;
            _welcomeSettings = welcomeOptions.Value;
        }

        public Task SendSuspiciousSessionAlertAsync(
            string toEmail,
            string userName,
            string suspiciousIp,
            string approveUrl,
            string revokeUrl,
            CancellationToken ct = default)
        {
            _logger.LogWarning(
                "SECURITY ALERT — Acceso sospechoso para el usuario '{UserName}' ({Email}) " +
                "desde la IP {SuspiciousIp}. " +
                "Aprobar: {ApproveUrl} | Revocar: {RevokeUrl}",
                userName, toEmail, suspiciousIp, approveUrl, revokeUrl);

            return Task.CompletedTask;
        }

        public Task SendWelcomeEmailAsync(
            string toEmail,
            string fullName,
            string username,
            string temporaryPassword,
            CancellationToken ct = default)
        {
            _logger.LogInformation(
                "WELCOME EMAIL — Para: {Email} | Nombre: {FullName} | App: {AppName} | " +
                "Usuario: {Username} | Contraseña temporal: {Password} | Login: {LoginUrl}",
                toEmail, fullName, _welcomeSettings.AppName, username, temporaryPassword, _welcomeSettings.LoginUrl);

            return Task.CompletedTask;
        }

        public Task SendPremiumReviewEmailAsync(
            string toEmail,
            string fullName,
            string planName,
            bool approved,
            string? rejectionReason,
            CancellationToken ct = default)
        {
            _logger.LogInformation(
                "PREMIUM REVIEW EMAIL — Para: {Email} | Nombre: {FullName} | Plan: {PlanName} | " +
                "Estado: {Status} | Motivo: {Reason}",
                toEmail, fullName, planName, approved ? "Aprobado" : "Rechazado", rejectionReason ?? "(sin motivo)");

            return Task.CompletedTask;
        }
    }
}
