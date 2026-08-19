namespace YachayPeru.API.Contracts.Auth.Request
{
    public sealed class RefreshTokenRequest
    {
        public string RefreshToken { get; set; } = default!;
    }
}
