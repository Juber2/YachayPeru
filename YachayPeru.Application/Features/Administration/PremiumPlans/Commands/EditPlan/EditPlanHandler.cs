using MediatR;
using YachayPeru.Application.Abstractions.Persistence;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Application.Common.Results;
using static YachayPeru.Application.Common.Results.ResultCodes;

namespace YachayPeru.Application.Features.Administration.PremiumPlans.Commands.EditPlan
{
    public class EditPlanHandler : IRequestHandler<EditPlanCommand, Result>
    {
        private readonly IPremiumPlanRepository planRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUser currentUser;

        public EditPlanHandler(IPremiumPlanRepository _planRepository, IUnitOfWork _unitOfWork, ICurrentUser _currentUser)
        {
            planRepository = _planRepository;
            unitOfWork = _unitOfWork;
            currentUser = _currentUser;
        }

        public async Task<Result> Handle(EditPlanCommand request, CancellationToken ct)
        {
            var plan = await planRepository.GetByIdAsync(request.Id, ct);
            if (plan is null)
                return Result.Failure("Plan no encontrado.", NotFound);

            await unitOfWork.BeginTransactionAsync(ct);

            plan.Name = request.Name;
            plan.Price = request.Price;
            plan.IsActive = request.IsActive;
            plan.UpdatedAt = DateTime.UtcNow;
            plan.UpdatedBy = currentUser.Id;
            planRepository.Update(plan);

            await planRepository.ReplacePlanFeaturesAsync(plan.Id, request.FeatureBenefitIds, ct);

            await unitOfWork.SaveChangesAsync(ct);
            await unitOfWork.CommitTransactionAsync(ct);

            return Result.Success();
        }
    }
}
