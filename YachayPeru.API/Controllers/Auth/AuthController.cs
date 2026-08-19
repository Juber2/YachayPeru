using MediatR;
using Microsoft.AspNetCore.Mvc;
using YachayPeru.API.Contracts.Auth.Request;
using YachayPeru.API.Extensions;
using YachayPeru.Application.Features.Authentication.Commands.ApproveSession;
using YachayPeru.Application.Features.Authentication.Commands.Auth;
using YachayPeru.Application.Features.Authentication.Commands.Register;
using YachayPeru.Application.Features.Authentication.Commands.RevokeSession;

namespace YachayPeru.API.Controllers.Auth
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _config;

        public AuthController(IMediator mediator, IConfiguration config)
        {
            _mediator = mediator;
            _config   = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
        {
            var command = new AuthCommand
            {
                Username  = request.Username,
                Password  = request.Password,
                IpAddress = GetClientIp(),
                UserAgent = Request.Headers.UserAgent.ToString()
            };

            var result = await _mediator.Send(command, ct);
            if (result.IsSuccess)
                SetRefreshTokenCookie(result.Value!.RefreshToken);

            return this.FromResult(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken ct)
        {
            var command = new RegisterCommand
            {
                FullName  = request.FullName,
                Email     = request.Email,
                Password  = request.Password,
                IpAddress = GetClientIp(),
                UserAgent = Request.Headers.UserAgent.ToString()
            };

            var result = await _mediator.Send(command, ct);
            if (result.IsSuccess)
                SetRefreshTokenCookie(result.Value!.RefreshToken);

            return this.FromResult(result);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(CancellationToken ct)
        {
            var command = new AuthCommand
            {
                RefreshToken = Request.Cookies["refreshToken"],
                IpAddress    = GetClientIp(),
                UserAgent    = Request.Headers.UserAgent.ToString()
            };

            var result = await _mediator.Send(command, ct);
            if (result.IsSuccess)
                SetRefreshTokenCookie(result.Value!.RefreshToken);

            return this.FromResult(result);
        }

        /// <summary>
        /// El usuario aprueba desde el link del email de alerta de seguridad.
        /// Emite un nuevo par de tokens y los retorna.
        /// </summary>
        [HttpGet("session/approve")]
        public async Task<IActionResult> ApproveSession([FromQuery] string token, CancellationToken ct)
        {
            var command = new ApproveSessionCommand { ApprovalToken = token };
            var result = await _mediator.Send(command, ct);
            return this.FromResult(result);
        }

        /// <summary>
        /// El usuario revoca desde el link del email de alerta de seguridad.
        /// Invalida todos los tokens activos del usuario.
        /// </summary>
        [HttpGet("session/revoke")]
        public async Task<IActionResult> RevokeSession([FromQuery] string token, CancellationToken ct)
        {
            var command = new RevokeSessionCommand { ApprovalToken = token };
            var result = await _mediator.Send(command, ct);
            return this.FromResult(result);
        }

        // ─── Helpers ──────────────────────────────────────────────────────────────

        private void SetRefreshTokenCookie(string token)
        {
            var secure   = _config.GetValue<bool>("CookieSettings:Secure");
            var sameSite = _config.GetValue<string>("CookieSettings:SameSite") switch
            {
                "None"   => SameSiteMode.None,
                "Lax"    => SameSiteMode.Lax,
                "Strict" => SameSiteMode.Strict,
                _        => SameSiteMode.Lax
            };

            var options = new CookieOptions
            {
                HttpOnly = true,
                Secure   = secure,
                SameSite = sameSite,
                Expires  = DateTimeOffset.UtcNow.AddDays(7)
            };

            Response.Cookies.Append("refreshToken", token, options);
        }

        private string GetClientIp()
        {
            var forwarded = Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(forwarded))
                return forwarded.Split(',')[0].Trim();

            return HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        }
    }
}
