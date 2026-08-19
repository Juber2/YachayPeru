using YachayPeru.Application.Abstractions.Persistence;
using YachayPeru.Application.Abstractions.Persistence.Assessment;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Application.Actions.Courses.Models;
using YachayPeru.Application.Common.Results;
using YachayPeru.Application.Features.Retos.Queries.GetRetoById;
using YachayPeru.Domain.Constants;
using YachayPeru.Domain.Entities.Assessment;
using static YachayPeru.Application.Common.Results.ResultCodes;

namespace YachayPeru.Application.Actions.Courses
{
    public class RetoListItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string StatusCode { get; set; } = string.Empty;
        public int QuestionCount { get; set; }
        public decimal TotalPoints { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class RetoLookupItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int CourseId { get; set; }
        public string RegionTitle { get; set; } = string.Empty;
    }

    public class RetoActions
    {
        private readonly IRetoRepository retoRepository;
        private readonly IRetoVersionRepository versionRepository;
        private readonly IRetoVersionQuestionRepository questionRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly ICurrentUser currentUser;

        public RetoActions(
            IRetoRepository _retoRepository,
            IRetoVersionRepository _versionRepository,
            IRetoVersionQuestionRepository _questionRepository,
            IUnitOfWork _unitOfWork,
            ICurrentUser _currentUser)
        {
            retoRepository = _retoRepository;
            versionRepository = _versionRepository;
            questionRepository = _questionRepository;
            unitOfWork = _unitOfWork;
            currentUser = _currentUser;
        }

        public async Task<IReadOnlyList<RetoListItem>> GetRetos(int courseId, CancellationToken ct)
        {
            var retos = await retoRepository.GetByCourseAsync(courseId, ct);
            var items = new List<RetoListItem>();

            foreach (var reto in retos)
            {
                var current = await versionRepository.GetDraftByRetoAsync(reto.Id, ct)
                    ?? await versionRepository.GetPublishedByRetoAsync(reto.Id, ct);
                if (current is null) continue;

                var questions = await questionRepository.GetByRetoVersionAsync(current.Id, ct);
                items.Add(new RetoListItem
                {
                    Id = reto.Id,
                    Title = current.Title,
                    StatusCode = current.StatusCode,
                    QuestionCount = questions.Count,
                    TotalPoints = questions.Sum(q => q.Points),
                    CreatedAt = reto.CreatedAt
                });
            }

            return items;
        }

        public async Task<IReadOnlyList<RetoLookupItem>> GetRetosLookup(CancellationToken ct)
        {
            var rows = await retoRepository.GetAllWithCourseAsync(ct);
            var items = new List<RetoLookupItem>();

            foreach (var row in rows)
            {
                var current = await versionRepository.GetDraftByRetoAsync(row.RetoId, ct)
                    ?? await versionRepository.GetPublishedByRetoAsync(row.RetoId, ct);
                if (current is null) continue;

                items.Add(new RetoLookupItem
                {
                    Id = row.RetoId,
                    Title = current.Title,
                    CourseId = row.CourseId,
                    RegionTitle = row.CourseTitle
                });
            }

            return items;
        }

        public async Task<Result<int>> CreateReto(int courseId, CancellationToken ct)
        {
            var reto = new Reto
            {
                CourseId = courseId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = currentUser.Id
            };
            await retoRepository.AddAsync(reto, ct);
            await unitOfWork.SaveChangesAsync(ct);

            var draft = new RetoVersion
            {
                RetoId = reto.Id,
                VersionNumber = 1,
                StatusCode = AppConstants.RetoVersionStatus.Draft,
                Title = string.Empty,
                PassingScore = 0,
                MaxAttempts = 3,
                ShowResultsAtEnd = true,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = currentUser.Id
            };
            await versionRepository.AddAsync(draft, ct);
            await unitOfWork.SaveChangesAsync(ct);

            return Result<int>.Success(reto.Id);
        }

        public async Task<Result> DeleteReto(int retoId, CancellationToken ct)
        {
            var reto = await retoRepository.GetByIdAsync(retoId, ct);
            if (reto is null)
                return Result.Failure("Reto no encontrado.", NotFound);

            reto.Deleted = true;
            reto.UpdatedAt = DateTime.UtcNow;
            reto.UpdatedBy = currentUser.Id;

            retoRepository.Update(reto);
            await unitOfWork.SaveChangesAsync(ct);

            return Result.Success();
        }

        public async Task<Result<int>> UpsertRetoSettings(UpsertRetoSettingsInput input, CancellationToken ct)
        {
            var draft = await versionRepository.GetDraftByRetoAsync(input.RetoId, ct);

            if (draft is null)
            {
                var nextVersion = await versionRepository.GetNextVersionNumberAsync(input.RetoId, ct);

                draft = new RetoVersion
                {
                    RetoId = input.RetoId,
                    VersionNumber = nextVersion,
                    StatusCode = AppConstants.RetoVersionStatus.Draft,
                    Title = input.Title,
                    PassingScore = input.PassingScore,
                    TimeLimitMinutes = input.TimeLimitMinutes,
                    MaxAttempts = input.MaxAttempts,
                    ShuffleQuestionOrder = input.ShuffleQuestionOrder,
                    ShuffleOptionOrder = input.ShuffleOptionOrder,
                    ShowResultsAtEnd = input.ShowResultsAtEnd,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = currentUser.Id
                };

                await versionRepository.AddAsync(draft, ct);
            }
            else
            {
                draft.Title = input.Title;
                draft.PassingScore = input.PassingScore;
                draft.TimeLimitMinutes = input.TimeLimitMinutes;
                draft.MaxAttempts = input.MaxAttempts;
                draft.ShuffleQuestionOrder = input.ShuffleQuestionOrder;
                draft.ShuffleOptionOrder = input.ShuffleOptionOrder;
                draft.ShowResultsAtEnd = input.ShowResultsAtEnd;
                draft.UpdatedAt = DateTime.UtcNow;
                draft.UpdatedBy = currentUser.Id;
                versionRepository.Update(draft);
            }

            await unitOfWork.SaveChangesAsync(ct);
            return Result<int>.Success(draft.Id);
        }

        public async Task<Result<int>> AddQuestion(AddRetoQuestionInput input, CancellationToken ct)
        {
            var version = await versionRepository.GetDraftByRetoAsync(input.RetoId, ct);
            if (version is null)
                return Result<int>.Failure("El reto no tiene un borrador activo.", NotFound);

            var nextOrder = await questionRepository.GetNextOrderIndexAsync(version.Id, ct);

            var question = new RetoVersionQuestion
            {
                RetoVersionId = version.Id,
                QuestionTypeCode = input.QuestionTypeCode,
                QuestionText = input.QuestionText,
                Points = input.Points,
                OrderIndex = nextOrder,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = currentUser.Id
            };

            await questionRepository.AddAsync(question, ct);
            await unitOfWork.SaveChangesAsync(ct);

            if (input.Choices.Count > 0)
            {
                var choices = input.Choices.Select(c => new RetoVersionQuestionChoice
                {
                    RetoVersionQuestionId = question.Id,
                    Text = c.Text,
                    IsCorrect = c.IsCorrect,
                    OrderIndex = c.OrderIndex,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = currentUser.Id
                });
                await questionRepository.AddChoicesAsync(choices, ct);
                await unitOfWork.SaveChangesAsync(ct);
            }

            if (input.Blanks.Count > 0)
            {
                var blanks = input.Blanks.Select(b => new RetoVersionQuestionBlank
                {
                    RetoVersionQuestionId = question.Id,
                    BlankIndex = b.BlankIndex,
                    CorrectAnswer = b.CorrectAnswer,
                    OrderIndex = b.OrderIndex,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = currentUser.Id
                });
                await questionRepository.AddBlanksAsync(blanks, ct);
                await unitOfWork.SaveChangesAsync(ct);
            }

            return Result<int>.Success(question.Id);
        }

        public async Task<Result<int>> EditQuestion(EditRetoQuestionInput input, CancellationToken ct)
        {
            var question = await questionRepository.GetByIdAsync(input.QuestionId, ct);
            if (question is null)
                return Result<int>.Failure("Pregunta no encontrada.", NotFound);

            var version = await versionRepository.GetByIdAsync(question.RetoVersionId, ct);
            if (version?.StatusCode != AppConstants.RetoVersionStatus.Draft)
                return Result<int>.Failure("Solo se pueden editar preguntas de un reto en borrador.", Conflict);

            question.QuestionText = input.QuestionText;
            question.Points = input.Points;
            question.UpdatedAt = DateTime.UtcNow;
            question.UpdatedBy = currentUser.Id;

            questionRepository.Update(question);

            await questionRepository.DeleteChoicesByQuestionAsync(question.Id, ct);

            if (input.Choices.Count > 0)
            {
                var choices = input.Choices.Select(c => new RetoVersionQuestionChoice
                {
                    RetoVersionQuestionId = question.Id,
                    Text = c.Text,
                    IsCorrect = c.IsCorrect,
                    OrderIndex = c.OrderIndex,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = currentUser.Id
                });
                await questionRepository.AddChoicesAsync(choices, ct);
            }

            await questionRepository.DeleteBlanksByQuestionAsync(question.Id, ct);

            if (input.Blanks.Count > 0)
            {
                var blanks = input.Blanks.Select(b => new RetoVersionQuestionBlank
                {
                    RetoVersionQuestionId = question.Id,
                    BlankIndex = b.BlankIndex,
                    CorrectAnswer = b.CorrectAnswer,
                    OrderIndex = b.OrderIndex,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = currentUser.Id
                });
                await questionRepository.AddBlanksAsync(blanks, ct);
            }

            await unitOfWork.SaveChangesAsync(ct);
            return Result<int>.Success(question.Id);
        }

        public async Task<Result> DeleteQuestion(int questionId, CancellationToken ct)
        {
            var question = await questionRepository.GetByIdAsync(questionId, ct);
            if (question is null)
                return Result.Failure("Pregunta no encontrada.", NotFound);

            var version = await versionRepository.GetByIdAsync(question.RetoVersionId, ct);
            if (version?.StatusCode != AppConstants.RetoVersionStatus.Draft)
                return Result.Failure("Solo se pueden eliminar preguntas de un reto en borrador.", Conflict);

            question.Deleted = true;
            question.UpdatedAt = DateTime.UtcNow;
            question.UpdatedBy = currentUser.Id;

            questionRepository.Update(question);
            await unitOfWork.SaveChangesAsync(ct);

            return Result.Success();
        }

        public async Task<Result> ReorderQuestions(ReorderQuestionsInput input, CancellationToken ct)
        {
            var version = await versionRepository.GetDraftByRetoAsync(input.RetoId, ct);
            if (version is null)
                return Result.Failure("El reto no tiene un borrador activo.", NotFound);

            foreach (var item in input.Items)
            {
                var q = await questionRepository.GetByIdAsync(item.QuestionId, ct);
                if (q is null || q.RetoVersionId != version.Id) continue;
                q.OrderIndex = item.OrderIndex;
                q.UpdatedAt = DateTime.UtcNow;
                q.UpdatedBy = currentUser.Id;
                questionRepository.Update(q);
            }

            await unitOfWork.SaveChangesAsync(ct);
            return Result.Success();
        }

        public async Task<Result<int>> CreateRetoDraft(int retoId, CancellationToken ct)
        {
            var existingDraft = await versionRepository.GetDraftByRetoAsync(retoId, ct);
            if (existingDraft is not null)
                return Result<int>.Failure("Ya existe un borrador activo.", Conflict);

            var published = await versionRepository.GetPublishedByRetoAsync(retoId, ct);
            if (published is null)
                return Result<int>.Failure("El reto no tiene ninguna versión publicada.", NotFound);

            var nextVersion = await versionRepository.GetNextVersionNumberAsync(retoId, ct);

            var newDraft = new RetoVersion
            {
                RetoId               = retoId,
                VersionNumber        = nextVersion,
                StatusCode           = AppConstants.RetoVersionStatus.Draft,
                Title                = published.Title,
                PassingScore         = published.PassingScore,
                TimeLimitMinutes     = published.TimeLimitMinutes,
                MaxAttempts          = published.MaxAttempts,
                ShuffleQuestionOrder = published.ShuffleQuestionOrder,
                ShuffleOptionOrder   = published.ShuffleOptionOrder,
                ShowResultsAtEnd     = published.ShowResultsAtEnd,
                CreatedAt            = DateTime.UtcNow,
                CreatedBy            = currentUser.Id
            };

            await versionRepository.AddAsync(newDraft, ct);
            await unitOfWork.SaveChangesAsync(ct);

            await CloneQuestionsAsync(published.Id, newDraft.Id, ct);

            await unitOfWork.SaveChangesAsync(ct);
            return Result<int>.Success(newDraft.Id);
        }

        public async Task<Result> DiscardRetoDraft(int retoId, CancellationToken ct)
        {
            var draft = await versionRepository.GetDraftByRetoAsync(retoId, ct);
            if (draft is null)
                return Result.Failure("No hay un borrador activo.", NotFound);

            draft.Deleted   = true;
            draft.UpdatedAt = DateTime.UtcNow;
            draft.UpdatedBy = currentUser.Id;
            versionRepository.Update(draft);
            await unitOfWork.SaveChangesAsync(ct);

            return Result.Success();
        }

        public async Task<IReadOnlyList<RetoVersionSummary>> GetRetoVersionHistory(int retoId, CancellationToken ct)
        {
            var versions = await versionRepository.GetHistoryByRetoAsync(retoId, ct);
            return versions.Select(v => new RetoVersionSummary
            {
                Id            = v.Id,
                VersionNumber = v.VersionNumber,
                StatusCode    = v.StatusCode,
                Title         = v.Title,
                CreatedAt     = v.CreatedAt
            }).ToList();
        }

        public async Task<Result<RetoVersionDetail>> GetRetoVersionDetail(int versionId, int retoId, CancellationToken ct)
        {
            var version = await versionRepository.GetByIdAsync(versionId, ct);
            if (version is null || version.RetoId != retoId)
                return Result<RetoVersionDetail>.Failure("Versión de reto no encontrada.", NotFound);

            var questions = await questionRepository.GetByRetoVersionAsync(version.Id, ct);
            var questionDtos = new List<QuestionDto>();
            foreach (var q in questions)
            {
                var choices = await questionRepository.GetChoicesByQuestionAsync(q.Id, ct);
                questionDtos.Add(new QuestionDto
                {
                    Id               = q.Id,
                    QuestionTypeCode = q.QuestionTypeCode,
                    QuestionText     = q.QuestionText,
                    Points           = q.Points,
                    OrderIndex       = q.OrderIndex,
                    Choices          = choices.Select(c => new ChoiceDto
                    {
                        Id         = c.Id,
                        Text       = c.Text,
                        IsCorrect  = c.IsCorrect,
                        OrderIndex = c.OrderIndex
                    }).ToList()
                });
            }

            return Result<RetoVersionDetail>.Success(new RetoVersionDetail
            {
                Id                   = version.Id,
                VersionNumber        = version.VersionNumber,
                StatusCode           = version.StatusCode,
                Title                = version.Title,
                PassingScore         = version.PassingScore,
                TimeLimitMinutes     = version.TimeLimitMinutes,
                MaxAttempts          = version.MaxAttempts,
                ShuffleQuestionOrder = version.ShuffleQuestionOrder,
                ShuffleOptionOrder   = version.ShuffleOptionOrder,
                ShowResultsAtEnd     = version.ShowResultsAtEnd,
                CreatedAt            = version.CreatedAt,
                Questions            = questionDtos
            });
        }

        private async Task CloneQuestionsAsync(int sourceVersionId, int targetVersionId, CancellationToken ct)
        {
            var questions = await questionRepository.GetByRetoVersionAsync(sourceVersionId, ct);
            foreach (var q in questions)
            {
                var copiedQuestion = new RetoVersionQuestion
                {
                    RetoVersionId    = targetVersionId,
                    QuestionTypeCode = q.QuestionTypeCode,
                    QuestionText     = q.QuestionText,
                    Points           = q.Points,
                    OrderIndex       = q.OrderIndex,
                    CreatedAt        = DateTime.UtcNow,
                    CreatedBy        = currentUser.Id
                };
                await questionRepository.AddAsync(copiedQuestion, ct);
                await unitOfWork.SaveChangesAsync(ct);

                var choices = await questionRepository.GetChoicesByQuestionAsync(q.Id, ct);
                if (choices.Count > 0)
                {
                    var copiedChoices = choices.Select(c => new RetoVersionQuestionChoice
                    {
                        RetoVersionQuestionId = copiedQuestion.Id,
                        Text                  = c.Text,
                        IsCorrect             = c.IsCorrect,
                        OrderIndex            = c.OrderIndex,
                        CreatedAt             = DateTime.UtcNow,
                        CreatedBy             = currentUser.Id
                    });
                    await questionRepository.AddChoicesAsync(copiedChoices, ct);
                }

                var blanks = await questionRepository.GetBlanksByQuestionAsync(q.Id, ct);
                if (blanks.Count > 0)
                {
                    var copiedBlanks = blanks.Select(b => new RetoVersionQuestionBlank
                    {
                        RetoVersionQuestionId = copiedQuestion.Id,
                        BlankIndex            = b.BlankIndex,
                        CorrectAnswer         = b.CorrectAnswer,
                        OrderIndex            = b.OrderIndex,
                        CreatedAt             = DateTime.UtcNow,
                        CreatedBy             = currentUser.Id
                    });
                    await questionRepository.AddBlanksAsync(copiedBlanks, ct);
                }
            }
        }

        public async Task<Result<int>> PublishReto(int retoId, CancellationToken ct)
        {
            var draft = await versionRepository.GetDraftByRetoAsync(retoId, ct);
            if (draft is null)
                return Result<int>.Failure("No hay un borrador para publicar.", NotFound);

            var questions = await questionRepository.GetByRetoVersionAsync(draft.Id, ct);
            if (questions.Count == 0)
                return Result<int>.Failure("El reto debe tener al menos una pregunta para publicarse.", Conflict);

            var published = await versionRepository.GetPublishedByRetoAsync(retoId, ct);
            if (published is not null)
            {
                published.StatusCode = AppConstants.RetoVersionStatus.Archived;
                published.UpdatedAt = DateTime.UtcNow;
                published.UpdatedBy = currentUser.Id;
                versionRepository.Update(published);
            }

            draft.StatusCode = AppConstants.RetoVersionStatus.Published;
            draft.UpdatedAt = DateTime.UtcNow;
            draft.UpdatedBy = currentUser.Id;

            versionRepository.Update(draft);
            await unitOfWork.SaveChangesAsync(ct);

            return Result<int>.Success(draft.Id);
        }
    }
}
