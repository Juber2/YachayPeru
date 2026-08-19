using MediatR;
using YachayPeru.Application.Abstractions.Persistence;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Application.Common.Results;
using static YachayPeru.Application.Common.Results.ResultCodes;

namespace YachayPeru.Application.Features.Administration.PremiumBenefits.Commands.EditBenefit
{
    public class EditBenefitHandler : IRequestHandler<EditBenefitCommand, Result>
    {
        private readonly IPremiumBenefitRepository benefitRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUser currentUser;

        public EditBenefitHandler(IPremiumBenefitRepository _benefitRepository, IUnitOfWork _unitOfWork, ICurrentUser _currentUser)
        {
            benefitRepository = _benefitRepository;
            unitOfWork = _unitOfWork;
            currentUser = _currentUser;
        }

        public async Task<Result> Handle(EditBenefitCommand request, CancellationToken ct)
        {
            var benefit = await benefitRepository.GetByIdAsync(request.Id, ct);
            if (benefit is null)
                return Result.Failure("Beneficio no encontrado.", NotFound);

            benefit.Code = request.Code;
            benefit.Label = request.Label;
            benefit.Description = request.Description;
            benefit.UpdatedAt = DateTime.UtcNow;
            benefit.UpdatedBy = currentUser.Id;

            benefitRepository.Update(benefit);
            await unitOfWork.SaveChangesAsync(ct);

            return Result.Success();
        }
    }
}
