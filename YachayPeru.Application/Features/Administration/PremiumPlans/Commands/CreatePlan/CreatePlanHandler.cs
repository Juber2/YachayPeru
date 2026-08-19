using MediatR;
using YachayPeru.Application.Abstractions.Persistence;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Application.Common.Results;
using YachayPeru.Domain.Entities.Content;

namespace YachayPeru.Application.Features.Administration.PremiumPlans.Commands.CreatePlan
{
    public class CreatePlanHandler : IRequestHandler<CreatePlanCommand, Result<int>>
    {
        private readonly IPremiumPlanRepository planRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUser currentUser;

        public CreatePlanHandler(IPremiumPlanRepository _planRepository, IUnitOfWork _unitOfWork, ICurrentUser _currentUser)
        {
            planRepository = _planRepository;
            unitOfWork = _unitOfWork;
            currentUser = _currentUser;
        }

        public async Task<Result<int>> Handle(CreatePlanCommand request, CancellationToken ct)
        {
            await unitOfWork.BeginTransactionAsync(ct);

            var plan = new PremiumPlan
            {
                Name = request.Name,
                Price = request.Price,
                IsActive = request.IsActive,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = currentUser.Id
            };

            await planRepository.AddAsync(plan, ct);
            await unitOfWork.SaveChangesAsync(ct);

            await planRepository.ReplacePlanFeaturesAsync(plan.Id, request.FeatureBenefitIds, ct);
            await unitOfWork.SaveChangesAsync(ct);

            await unitOfWork.CommitTransactionAsync(ct);

            return Result<int>.Success(plan.Id);
        }
    }
}
