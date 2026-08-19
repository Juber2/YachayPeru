using MediatR;
using YachayPeru.Application.Abstractions.Persistence.Assessment;
using YachayPeru.Application.Common.Results;
using static YachayPeru.Application.Common.Results.ResultCodes;

namespace YachayPeru.Application.Features.Aprendiz.Retos.Queries.GetRetoById
{
    public class GetRetoByIdHandler : IRequestHandler<GetRetoByIdQuery, Result<AprendizRetoPlay>>
    {
        private readonly IRetoVersionRepository versionRepository;
        private readonly IRetoVersionQuestionRepository questionRepository;

        public GetRetoByIdHandler(IRetoVersionRepository _versionRepository, IRetoVersionQuestionRepository _questionRepository)
        {
            versionRepository = _versionRepository;
            questionRepository = _questionRepository;
        }

        public async Task<Result<AprendizRetoPlay>> Handle(GetRetoByIdQuery request, CancellationToken ct)
        {
            var published = await versionRepository.GetPublishedByRetoAsync(request.RetoId, ct);
            if (published is null)
                return Result<AprendizRetoPlay>.Failure("El reto no está disponible.", NotFound);

            var questions = await questionRepository.GetByRetoVersionAsync(published.Id, ct);
            var questionDtos = new List<AprendizQuestion>();

            foreach (var q in questions)
            {
                var choices = await questionRepository.GetChoicesByQuestionAsync(q.Id, ct);
                var blanks = await questionRepository.GetBlanksByQuestionAsync(q.Id, ct);

                questionDtos.Add(new AprendizQuestion
                {
                    Id = q.Id,
                    QuestionTypeCode = q.QuestionTypeCode,
                    QuestionText = q.QuestionText,
                    Points = q.Points,
                    OrderIndex = q.OrderIndex,
                    Choices = choices.Select(c => new AprendizChoice
                    {
                        Id = c.Id,
                        Text = c.Text,
                        OrderIndex = c.OrderIndex
                    }).ToList(),
                    BlanksCount = blanks.Count
                });
            }

            return Result<AprendizRetoPlay>.Success(new AprendizRetoPlay
            {
                Id = request.RetoId,
                Title = published.Title,
                TimeLimitMinutes = published.TimeLimitMinutes,
                PassingScore = published.PassingScore,
                ShuffleQuestionOrder = published.ShuffleQuestionOrder,
                ShuffleOptionOrder = published.ShuffleOptionOrder,
                Questions = questionDtos
            });
        }
    }
}
