using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Application.Common.Settings;

namespace YachayPeru.Infrastructure.Services
{
    public sealed class SmtpEmailService : IEmailService
    {
        private readonly SmtpSettings        _smtp;
        private readonly WelcomeEmailSettings _welcome;

        public SmtpEmailService(IOptions<SmtpSettings> smtp, IOptions<WelcomeEmailSettings> welcome)
        {
            _smtp    = smtp.Value;
            _welcome = welcome.Value;
        }

        public async Task SendWelcomeEmailAsync(
            string toEmail,
            string fullName,
            string username,
            string temporaryPassword,
            CancellationToken ct = default)
        {
            var body = $"""
                <h2>Bienvenido a {_welcome.AppName}, {fullName}</h2>
                <p>Tu cuenta ha sido creada. Aquí están tus credenciales de acceso:</p>
                <ul>
                    <li><strong>Usuario:</strong> {username}</li>
                    <li><strong>Contraseña temporal:</strong> {temporaryPassword}</li>
                </ul>
                <p>Ingresá desde: <a href="{_welcome.LoginUrl}">{_welcome.LoginUrl}</a></p>
                <p>Te recomendamos cambiar tu contraseña al ingresar por primera vez.</p>
                """;

            await SendAsync(toEmail, fullName, $"Bienvenido a {_welcome.AppName}", body, ct);
        }

        public async Task SendSuspiciousSessionAlertAsync(
            string toEmail,
            string userName,
            string suspiciousIp,
            string approveUrl,
            string revokeUrl,
            CancellationToken ct = default)
        {
            var body = $"""
                <h2>Alerta de seguridad</h2>
                <p>Hola <strong>{userName}</strong>, detectamos un acceso desde una IP desconocida: <strong>{suspiciousIp}</strong>.</p>
                <p>
                    <a href="{approveUrl}" style="color:green">✅ Fui yo, aprobar acceso</a><br/><br/>
                    <a href="{revokeUrl}" style="color:red">🚫 No fui yo, revocar sesión</a>
                </p>
                """;

            await SendAsync(toEmail, userName, "Alerta de seguridad — acceso sospechoso", body, ct);
        }

        public async Task SendPremiumReviewEmailAsync(
            string toEmail,
            string fullName,
            string planName,
            bool approved,
            string? rejectionReason,
            CancellationToken ct = default)
        {
            var subject = approved ? "Tu solicitud Premium fue aprobada 🎉" : "Tu comprobante Premium fue rechazado";

            var body = approved
                ? $"""
                    <h2>¡Felicidades, {fullName}!</h2>
                    <p>Tu comprobante de pago para el plan <strong>{planName}</strong> fue aprobado — ya tenés Premium activo.</p>
                    <p>Ingresá desde: <a href="{_welcome.LoginUrl}">{_welcome.LoginUrl}</a></p>
                    """
                : $"""
                    <h2>Hola {fullName}</h2>
                    <p>Tu comprobante de pago para el plan <strong>{planName}</strong> no pudo confirmarse.</p>
                    {(string.IsNullOrWhiteSpace(rejectionReason) ? "" : $"<p><strong>Motivo:</strong> {rejectionReason}</p>")}
                    <p>Podés volver a intentarlo desde la app subiendo un nuevo comprobante: <a href="{_welcome.LoginUrl}">{_welcome.LoginUrl}</a></p>
                    """;

            await SendAsync(toEmail, fullName, subject, body, ct);
        }

        private async Task SendAsync(string toEmail, string toName, string subject, string htmlBody, CancellationToken ct)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_smtp.FromName, _smtp.From));
            message.To.Add(new MailboxAddress(toName, toEmail));
            message.Subject = subject;
            message.Body    = new TextPart("html") { Text = htmlBody };

            using var client = new SmtpClient();
            await client.ConnectAsync(_smtp.Host, _smtp.Port, SecureSocketOptions.StartTls, ct);
            await client.AuthenticateAsync(_smtp.Username, _smtp.Password, ct);
            await client.SendAsync(message, ct);
            await client.DisconnectAsync(true, ct);
        }
    }
}
