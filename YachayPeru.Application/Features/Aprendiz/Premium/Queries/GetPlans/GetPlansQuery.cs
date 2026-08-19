using MediatR;

namespace YachayPeru.Application.Features.Aprendiz.Premium.Queries.GetPlans
{
    public sealed record GetPlansQuery(int UserId) : IRequest<AprendizPremiumPlans>;

    public sealed record AprendizPremiumPlans
    {
        public bool IsPremiumUser { get; init; }
        public int? SelectedPlanId { get; init; }
        public string? WaitlistStatus { get; init; }
        public string? RejectionReason { get; init; }
        public bool HasUnseenReview { get; init; }
        public IReadOnlyList<AprendizPremiumPlanCard> Plans { get; init; } = new List<AprendizPremiumPlanCard>();
    }

    public sealed record AprendizPremiumPlanCard
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public decimal Price { get; init; }
        public IReadOnlyList<string> Features { get; init; } = new List<string>();
    }
}
