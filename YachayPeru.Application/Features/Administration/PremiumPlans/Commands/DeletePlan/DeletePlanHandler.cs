using MediatR;
using YachayPeru.Application.Abstractions.Persistence;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Application.Common.Results;
using static YachayPeru.Application.Common.Results.ResultCodes;

namespace YachayPeru.Application.Features.Administration.PremiumPlans.Commands.DeletePlan
{
    public class DeletePlanHandler : IRequestHandler<DeletePlanCommand, Result>
    {
        private readonly IPremiumPlanRepository planRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUser currentUser;

        public DeletePlanHandler(IPremiumPlanRepository _planRepository, IUnitOfWork _unitOfWork, ICurrentUser _currentUser)
        {
            planRepository = _planRepository;
            unitOfWork = _unitOfWork;
            currentUser = _currentUser;
        }

        public async Task<Result> Handle(DeletePlanCommand request, CancellationToken ct)
        {
            var plan = await planRepository.GetByIdAsync(request.Id, ct);
            if (plan is null)
                return Result.Failure("Plan no encontrado.", NotFound);

            if (await planRepository.IsUsedInWaitlistAsync(plan.Id, ct))
                return Result.Failure("No se puede eliminar: hay usuarios anotados en la waitlist de este plan.", Conflict);

            plan.Deleted = true;
            plan.UpdatedAt = DateTime.UtcNow;
            plan.UpdatedBy = currentUser.Id;

            planRepository.Update(plan);
            await unitOfWork.SaveChangesAsync(ct);

            return Result.Success();
        }
    }
}
