namespace YachayPeru.API.Contracts.Administration.Premium.Request
{
    public record UpsertPremiumPlanRequest
    {
        public string Name { get; init; } = string.Empty;
        public decimal Price { get; init; }
        public bool IsActive { get; init; } = true;
        public List<int> FeatureBenefitIds { get; init; } = new();
    }

    public record ReviewWaitlistRequest
    {
        public string Status { get; init; } = string.Empty;
        public string? RejectionReason { get; init; }
    }

    public record UpsertPremiumBenefitRequest
    {
        public string Code { get; init; } = string.Empty;
        public string Label { get; init; } = string.Empty;
        public string? Description { get; init; }
    }
}
