namespace YachayPeru.API.Contracts.Aprendiz.Premium.Response
{
    public record AprendizPremiumPlanCardResponse
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public decimal Price { get; init; }
        public List<string> Features { get; init; } = new();
    }

    public record AprendizPremiumPlansResponse
    {
        public bool IsPremiumUser { get; init; }
        public int? SelectedPlanId { get; init; }
        public string? WaitlistStatus { get; init; }
        public string? RejectionReason { get; init; }
        public bool HasUnseenReview { get; init; }
        public List<AprendizPremiumPlanCardResponse> Plans { get; init; } = new();
    }
}
