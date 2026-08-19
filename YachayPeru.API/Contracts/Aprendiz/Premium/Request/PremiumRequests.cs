namespace YachayPeru.API.Contracts.Aprendiz.Premium.Request
{
    public record PostWaitlistRequest
    {
        public int PlanId { get; init; }
        public string PaymentMethod { get; init; } = string.Empty;
    }
}
