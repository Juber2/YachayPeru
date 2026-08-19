namespace YachayPeru.Application.Features.Authentication.Response
{
    public sealed record AuthResult
    {
        public string AccessToken { get; init; } = default!;
        public string RefreshToken { get; init; } = default!;
        public DateTime AccessTokenExpiresAt { get; init; }

        public int UserId { get; init; }
        public string Username { get; init; } = default!;
        public string FullName { get; init; } = default!;
        public string? Email { get; init; }
        public string UserTypeCode { get; init; } = default!;

        public IReadOnlyList<string> Permissions { get; init; } = [];
        public IReadOnlyList<string> Roles { get; init; } = [];
    }
}
