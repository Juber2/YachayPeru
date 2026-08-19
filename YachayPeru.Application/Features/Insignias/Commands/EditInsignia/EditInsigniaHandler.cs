using MediatR;
using YachayPeru.Application.Abstractions.Persistence;
using YachayPeru.Application.Abstractions.Persistence.Content;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Application.Common.Results;
using static YachayPeru.Application.Common.Results.ResultCodes;

namespace YachayPeru.Application.Features.Insignias.Commands.EditInsignia
{
    public class EditInsigniaHandler : IRequestHandler<EditInsigniaCommand, Result>
    {
        private readonly IInsigniaRepository repository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUser currentUser;

        public EditInsigniaHandler(IInsigniaRepository _repository, IUnitOfWork _unitOfWork, ICurrentUser _currentUser)
        {
            repository = _repository;
            unitOfWork = _unitOfWork;
            currentUser = _currentUser;
        }

        public async Task<Result> Handle(EditInsigniaCommand request, CancellationToken ct)
        {
            var insignia = await repository.GetByIdAsync(request.Id, ct);
            if (insignia is null)
                return Result.Failure("Insignia no encontrada.", NotFound);

            insignia.Name = request.Name;
            insignia.Description = request.Description;
            insignia.IsActive = request.IsActive;
            insignia.MinPoints = request.MinPoints;
            insignia.MinRetosCompleted = request.MinRetosCompleted;
            insignia.MinPerfectRetos = request.MinPerfectRetos;
            insignia.RequireAllQuestionTypes = request.RequireAllQuestionTypes;
            insignia.MinLevel = request.MinLevel;
            insignia.RequirePremium = request.RequirePremium;
            insignia.MinLoginStreakDays = request.MinLoginStreakDays;
            insignia.MinAnswerStreak = request.MinAnswerStreak;
            insignia.MinRegionsExplored = request.MinRegionsExplored;
            insignia.RequiredZoneCode = request.RequiredZoneCode;
            insignia.MinZoneRegionsExplored = request.MinZoneRegionsExplored;
            insignia.UpdatedAt = DateTime.UtcNow;
            insignia.UpdatedBy = currentUser.Id;

            repository.Update(insignia);
            await repository.ReplaceRequiredRegionsAsync(insignia.Id, request.RequiredRegionIds, ct);
            await repository.ReplaceRequiredRetosAsync(insignia.Id, request.RequiredRetoIds, ct);
            await unitOfWork.SaveChangesAsync(ct);

            return Result.Success();
        }
    }
}
