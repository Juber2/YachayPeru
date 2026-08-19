using MediatR;
using YachayPeru.Application.Abstractions.Persistence;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Application.Common.Results;
using static YachayPeru.Application.Common.Results.ResultCodes;

namespace YachayPeru.Application.Features.Administration.PremiumBenefits.Commands.DeleteBenefit
{
    public class DeleteBenefitHandler : IRequestHandler<DeleteBenefitCommand, Result>
    {
        private readonly IPremiumBenefitRepository benefitRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUser currentUser;

        public DeleteBenefitHandler(IPremiumBenefitRepository _benefitRepository, IUnitOfWork _unitOfWork, ICurrentUser _currentUser)
        {
            benefitRepository = _benefitRepository;
            unitOfWork = _unitOfWork;
            currentUser = _currentUser;
        }

        public async Task<Result> Handle(DeleteBenefitCommand request, CancellationToken ct)
        {
            var benefit = await benefitRepository.GetByIdAsync(request.Id, ct);
            if (benefit is null)
                return Result.Failure("Beneficio no encontrado.", NotFound);

            if (await benefitRepository.IsUsedInAnyPlanAsync(benefit.Id, ct))
                return Result.Failure("No se puede eliminar: el beneficio está asignado a un plan. Quitalo del plan Free/Premium primero.", Conflict);

            benefit.Deleted = true;
            benefit.UpdatedAt = DateTime.UtcNow;
            benefit.UpdatedBy = currentUser.Id;

            benefitRepository.Update(benefit);
            await unitOfWork.SaveChangesAsync(ct);

            return Result.Success();
        }
    }
}
