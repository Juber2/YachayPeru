using MediatR;
using YachayPeru.Application.Abstractions.Persistence.Assessment;
using YachayPeru.Application.Common.Results;
using static YachayPeru.Application.Common.Results.ResultCodes;

namespace YachayPeru.Application.Features.Retos.Queries.GetRetoById
{
    public class GetRetoByIdHandler : IRequestHandler<GetRetoByIdQuery, Result<RetoDetail>>
    {
        private readonly IRetoVersionRepository versionRepository;
        private readonly IRetoVersionQuestionRepository questionRepository;

        public GetRetoByIdHandler(
            IRetoVersionRepository _versionRepository,
            IRetoVersionQuestionRepository _questionRepository)
        {
            versionRepository = _versionRepository;
            questionRepository = _questionRepository;
        }

        public async Task<Result<RetoDetail>> Handle(GetRetoByIdQuery request, CancellationToken cancellationToken)
        {
            var draft = await versionRepository.GetDraftByRetoAsync(request.RetoId, cancellationToken);
            var version = draft ?? await versionRepository.GetPublishedByRetoAsync(request.RetoId, cancellationToken);

            if (version is null)
                return Result<RetoDetail>.Failure("El reto no tiene ninguna versión configurada.", NotFound);

            var questions = await questionRepository.GetByRetoVersionAsync(version.Id, cancellationToken);

            var questionDtos = new List<QuestionDto>();
            foreach (var q in questions)
            {
                var choices = await questionRepository.GetChoicesByQuestionAsync(q.Id, cancellationToken);
                questionDtos.Add(new QuestionDto
                {
                    Id = q.Id,
                    QuestionTypeCode = q.QuestionTypeCode,
                    QuestionText = q.QuestionText,
                    Points = q.Points,
                    OrderIndex = q.OrderIndex,
                    Choices = choices.Select(c => new ChoiceDto
                    {
                        Id = c.Id,
                        Text = c.Text,
                        IsCorrect = c.IsCorrect,
                        OrderIndex = c.OrderIndex
                    }).ToList()
                });
            }

            return Result<RetoDetail>.Success(new RetoDetail
            {
                Id = version.Id,
                VersionNumber = version.VersionNumber,
                StatusCode = version.StatusCode,
                Title = version.Title,
                PassingScore = version.PassingScore,
                TimeLimitMinutes = version.TimeLimitMinutes,
                MaxAttempts = version.MaxAttempts,
                ShuffleQuestionOrder = version.ShuffleQuestionOrder,
                ShuffleOptionOrder = version.ShuffleOptionOrder,
                ShowResultsAtEnd = version.ShowResultsAtEnd,
                Questions = questionDtos
            });
        }
    }
}
