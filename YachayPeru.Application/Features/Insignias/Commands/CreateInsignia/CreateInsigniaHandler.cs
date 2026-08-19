using MediatR;
using YachayPeru.Application.Abstractions.Persistence;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Application.Common.Results;
using YachayPeru.Domain.Entities.Content;

namespace YachayPeru.Application.Features.Insignias.Commands.CreateInsignia
{
    public class CreateInsigniaHandler : IRequestHandler<CreateInsigniaCommand, Result<int>>
    {
        private readonly IInsigniaRepository repository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUser currentUser;

        public CreateInsigniaHandler(IInsigniaRepository _repository, IUnitOfWork _unitOfWork, ICurrentUser _currentUser)
        {
            repository = _repository;
            unitOfWork = _unitOfWork;
            currentUser = _currentUser;
        }

        public async Task<Result<int>> Handle(CreateInsigniaCommand request, CancellationToken ct)
        {
            var insignia = new Insignia
            {
                Name = request.Name,
                Description = request.Description,
                IsActive = request.IsActive,
                MinPoints = request.MinPoints,
                MinRetosCompleted = request.MinRetosCompleted,
                MinPerfectRetos = request.MinPerfectRetos,
                RequireAllQuestionTypes = request.RequireAllQuestionTypes,
                MinLevel = request.MinLevel,
                RequirePremium = request.RequirePremium,
                MinLoginStreakDays = request.MinLoginStreakDays,
                MinAnswerStreak = request.MinAnswerStreak,
                MinRegionsExplored = request.MinRegionsExplored,
                RequiredZoneCode = request.RequiredZoneCode,
                MinZoneRegionsExplored = request.MinZoneRegionsExplored,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = currentUser.Id
            };

            await repository.AddAsync(insignia, ct);
            await unitOfWork.SaveChangesAsync(ct);

            await repository.ReplaceRequiredRegionsAsync(insignia.Id, request.RequiredRegionIds, ct);
            await repository.ReplaceRequiredRetosAsync(insignia.Id, request.RequiredRetoIds, ct);
            await unitOfWork.SaveChangesAsync(ct);

            return Result<int>.Success(insignia.Id);
        }
    }
}
