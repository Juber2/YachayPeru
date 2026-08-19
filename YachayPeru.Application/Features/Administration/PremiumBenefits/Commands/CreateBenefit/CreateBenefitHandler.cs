using MediatR;
using YachayPeru.Application.Abstractions.Persistence;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Application.Common.Results;
using YachayPeru.Domain.Entities.Content;

namespace YachayPeru.Application.Features.Administration.PremiumBenefits.Commands.CreateBenefit
{
    public class CreateBenefitHandler : IRequestHandler<CreateBenefitCommand, Result<int>>
    {
        private readonly IPremiumBenefitRepository benefitRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUser currentUser;

        public CreateBenefitHandler(IPremiumBenefitRepository _benefitRepository, IUnitOfWork _unitOfWork, ICurrentUser _currentUser)
        {
            benefitRepository = _benefitRepository;
            unitOfWork = _unitOfWork;
            currentUser = _currentUser;
        }

        public async Task<Result<int>> Handle(CreateBenefitCommand request, CancellationToken ct)
        {
            var benefit = new PremiumBenefit
            {
                Code = request.Code,
                Label = request.Label,
                Description = request.Description,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = currentUser.Id
            };

            await benefitRepository.AddAsync(benefit, ct);
            await unitOfWork.SaveChangesAsync(ct);

            return Result<int>.Success(benefit.Id);
        }
    }
}
