using MediatR;
using YachayPeru.Application.Abstractions.Persistence;
using YachayPeru.Application.Abstractions.Persistence.Aprendiz;
using YachayPeru.Application.Abstractions.Persistence.Assessment;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Application.Common;
using YachayPeru.Application.Common.Results;
using YachayPeru.Domain.Constants;
using YachayPeru.Domain.Entities.Aprendiz;
using static YachayPeru.Application.Common.Results.ResultCodes;

namespace YachayPeru.Application.Features.Aprendiz.Retos.Commands.PostIntento
{
    public class PostIntentoHandler : IRequestHandler<PostIntentoCommand, Result<RetoAttemptResult>>
    {
        private readonly IRetoRepository retoRepository;
        private readonly IRetoVersionRepository versionRepository;
        private readonly IRetoVersionQuestionRepository questionRepository;
        private readonly IRetoAttemptRepository attemptRepository;
        private readonly IAprendizProfileRepository profileRepository;
        private readonly IAprendizActivityLogRepository activityRepository;
        private readonly IInsigniaEvaluator insigniaEvaluator;
        private readonly IUnitOfWork unitOfWork;

        public PostIntentoHandler(
            IRetoRepository _retoRepository,
            IRetoVersionRepository _versionRepository,
            IRetoVersionQuestionRepository _questionRepository,
            IRetoAttemptRepository _attemptRepository,
            IAprendizProfileRepository _profileRepository,
            IAprendizActivityLogRepository _activityRepository,
            IInsigniaEvaluator _insigniaEvaluator,
            IUnitOfWork _unitOfWork)
        {
            retoRepository = _retoRepository;
            versionRepository = _versionRepository;
            questionRepository = _questionRepository;
            attemptRepository = _attemptRepository;
            profileRepository = _profileRepository;
            activityRepository = _activityRepository;
            insigniaEvaluator = _insigniaEvaluator;
            unitOfWork = _unitOfWork;
        }

        public async Task<Result<RetoAttemptResult>> Handle(PostIntentoCommand request, CancellationToken ct)
        {
            var reto = await retoRepository.GetByIdAsync(request.RetoId, ct);
            if (reto is null)
                return Result<RetoAttemptResult>.Failure("Reto no encontrado.", NotFound);

            var published = await versionRepository.GetPublishedByRetoAsync(request.RetoId, ct);
            if (published is null)
                return Result<RetoAttemptResult>.Failure("El reto no está disponible.", NotFound);

            var attemptsUsed = await attemptRepository.CountByUserAndRetoAsync(request.UserId, request.RetoId, ct);
            if (attemptsUsed >= published.MaxAttempts)
                return Result<RetoAttemptResult>.Failure("Alcanzaste el máximo de intentos para este reto.", Conflict);

            var hadPassedBefore = await attemptRepository.HasPassedAsync(request.UserId, request.RetoId, ct);

            var questions = await questionRepository.GetByRetoVersionAsync(published.Id, ct);
            var answersByQuestion = request.Answers.ToDictionary(a => a.QuestionId);

            var perQuestion = new List<PerQuestionResult>();
            decimal earnedPoints = 0;
            decimal totalPoints = 0;
            var correctCount = 0;

            foreach (var q in questions)
            {
                totalPoints += q.Points;
                answersByQuestion.TryGetValue(q.Id, out var answer);

                bool isCorrect;
                List<int>? correctChoiceIds = null;
                List<string>? correctBlankAnswers = null;

                if (q.QuestionTypeCode == AppConstants.QuestionTypeCode.FillBlank)
                {
                    var blanks = await questionRepository.GetBlanksByQuestionAsync(q.Id, ct);
                    correctBlankAnswers = blanks.OrderBy(b => b.OrderIndex).Select(b => b.CorrectAnswer).ToList();

                    var given = answer?.BlankAnswers ?? [];
                    isCorrect = blanks.Count > 0
                        && given.Count == blanks.Count
                        && blanks.OrderBy(b => b.OrderIndex)
                                 .Select(b => b.CorrectAnswer.Trim().ToLowerInvariant())
                                 .SequenceEqual(given.Select(g => g.Trim().ToLowerInvariant()));
                }
                else
                {
                    var choices = await questionRepository.GetChoicesByQuestionAsync(q.Id, ct);
                    var correctIds = choices.Where(c => c.IsCorrect).Select(c => c.Id).ToHashSet();
                    correctChoiceIds = correctIds.ToList();

                    var selected = (answer?.SelectedChoiceIds ?? []).ToHashSet();
                    isCorrect = correctIds.Count > 0 && correctIds.SetEquals(selected);
                }

                if (isCorrect)
                {
                    earnedPoints += q.Points;
                    correctCount++;
                }

                perQuestion.Add(new PerQuestionResult
                {
                    QuestionId = q.Id,
                    IsCorrect = isCorrect,
                    CorrectChoiceIds = correctChoiceIds,
                    CorrectBlankAnswers = correctBlankAnswers
                });
            }

            var scorePercent = totalPoints > 0 ? earnedPoints / totalPoints * 100 : 0;
            var passed = totalPoints > 0 && scorePercent >= published.PassingScore;

            await attemptRepository.AddAsync(new RetoAttempt
            {
                UserId = request.UserId,
                RetoId = request.RetoId,
                RetoVersionId = published.Id,
                EarnedPoints = earnedPoints,
                TotalPoints = totalPoints,
                Passed = passed,
                CorrectCount = correctCount,
                TotalQuestions = questions.Count,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = request.UserId
            }, ct);

            var profile = await profileRepository.GetByUserIdAsync(request.UserId, ct);
            if (profile is null)
            {
                profile = new Domain.Entities.Aprendiz.AprendizProfile
                {
                    UserId = request.UserId,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = request.UserId
                };
                await profileRepository.AddAsync(profile, ct);
            }

            // Racha de aciertos consecutivos (global, no por reto) — se actualiza en cada intento,
            // apruebe o no el reto completo, para que las insignias de racha puedan completarse
            // aunque el intento en sí no alcance el puntaje mínimo.
            foreach (var q in perQuestion)
            {
                if (q.IsCorrect)
                {
                    profile.CurrentAnswerStreak++;
                    profile.BestAnswerStreak = Math.Max(profile.BestAnswerStreak, profile.CurrentAnswerStreak);
                }
                else
                {
                    profile.CurrentAnswerStreak = 0;
                }
            }

            if (passed && !hadPassedBefore)
            {
                profile.Points += (int)Math.Round(earnedPoints);
                profile.Level = AprendizLevelCalculator.CalculateLevel(profile.Points);

                await activityRepository.AddAsync(new AprendizActivityLog
                {
                    UserId = request.UserId,
                    Text = $"Completaste el reto de {published.Title}",
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = request.UserId
                }, ct);
            }

            profile.UpdatedAt = DateTime.UtcNow;
            profile.UpdatedBy = request.UserId;
            profileRepository.Update(profile);

            await unitOfWork.SaveChangesAsync(ct);

            await insigniaEvaluator.EvaluateAsync(request.UserId, ct);

            return Result<RetoAttemptResult>.Success(new RetoAttemptResult
            {
                EarnedPoints = earnedPoints,
                TotalPoints = totalPoints,
                Passed = passed,
                CorrectCount = correctCount,
                TotalQuestions = questions.Count,
                PerQuestion = perQuestion
            });
        }
    }
}
