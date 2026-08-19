using MediatR;
using YachayPeru.Application.Abstractions.Persistence.Aprendiz;
using YachayPeru.Application.Abstractions.Persistence.Assessment;
using YachayPeru.Application.Abstractions.Persistence.Learning;
using YachayPeru.Domain.Entities.Learning;

namespace YachayPeru.Application.Features.Aprendiz.Retos.Queries.GetRetos
{
    public class GetRetosHandler : IRequestHandler<GetRetosQuery, IReadOnlyList<AprendizRetoListItem>>
    {
        private readonly ICourseRepository courseRepository;
        private readonly IRetoRepository retoRepository;
        private readonly IRetoVersionRepository versionRepository;
        private readonly IRetoVersionQuestionRepository questionRepository;
        private readonly IRetoAttemptRepository attemptRepository;

        public GetRetosHandler(
            ICourseRepository _courseRepository,
            IRetoRepository _retoRepository,
            IRetoVersionRepository _versionRepository,
            IRetoVersionQuestionRepository _questionRepository,
            IRetoAttemptRepository _attemptRepository)
        {
            courseRepository = _courseRepository;
            retoRepository = _retoRepository;
            versionRepository = _versionRepository;
            questionRepository = _questionRepository;
            attemptRepository = _attemptRepository;
        }

        public async Task<IReadOnlyList<AprendizRetoListItem>> Handle(GetRetosQuery request, CancellationToken ct)
        {
            List<Course> regions;
            if (request.RegionId is not null)
            {
                var region = await courseRepository.GetByIdAsync(request.RegionId.Value, ct);
                regions = region is null ? [] : [region];
            }
            else
            {
                regions = (await courseRepository.ListAsync(c => c.IsActive, ct)).ToList();
            }

            var items = new List<AprendizRetoListItem>();

            foreach (var region in regions)
            {
                var retos = await retoRepository.GetByCourseAsync(region.Id, ct);
                foreach (var reto in retos)
                {
                    var published = await versionRepository.GetPublishedByRetoAsync(reto.Id, ct);
                    if (published is null) continue;

                    var questions = await questionRepository.GetByRetoVersionAsync(published.Id, ct);
                    var bestAttempt = await attemptRepository.GetBestByUserAndRetoAsync(request.UserId, reto.Id, ct);

                    items.Add(new AprendizRetoListItem
                    {
                        Id = reto.Id,
                        Title = published.Title,
                        RegionId = region.Id,
                        RegionTitle = region.Title,
                        QuestionCount = questions.Count,
                        TotalPoints = questions.Sum(q => q.Points),
                        EarnedPoints = bestAttempt?.EarnedPoints ?? 0,
                        IsCompleted = await attemptRepository.HasPassedAsync(request.UserId, reto.Id, ct),
                        AttemptsUsed = await attemptRepository.CountByUserAndRetoAsync(request.UserId, reto.Id, ct),
                        MaxAttempts = published.MaxAttempts
                    });
                }
            }

            return items;
        }
    }
}
