namespace YachayPeru.API.Contracts.Administration.Premium.Response
{
    public record PremiumBenefitResponse
    {
        public int Id { get; init; }
        public string Code { get; init; } = string.Empty;
        public string Label { get; init; } = string.Empty;
        public string? Description { get; init; }
    }

    public record PremiumPlanResponse
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public decimal Price { get; init; }
        public bool IsActive { get; init; }
        public List<PremiumBenefitResponse> Features { get; init; } = new();
    }

    public record PremiumWaitlistEntryResponse
    {
        public int Id { get; init; }
        public int UserId { get; init; }
        public string UserFullName { get; init; } = string.Empty;
        public string UserEmail { get; init; } = string.Empty;
        public int PlanId { get; init; }
        public string PlanName { get; init; } = string.Empty;
        public string PaymentMethod { get; init; } = string.Empty;
        public string? ReceiptUrl { get; init; }
        public string Status { get; init; } = string.Empty;
        public string? RejectionReason { get; init; }
        public DateTime JoinedAt { get; init; }
    }
}
