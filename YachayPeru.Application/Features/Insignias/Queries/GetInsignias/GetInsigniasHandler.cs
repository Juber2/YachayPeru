using MediatR;
using YachayPeru.Application.Abstractions.Persistence.Content;

namespace YachayPeru.Application.Features.Insignias.Queries.GetInsignias
{
    public class GetInsigniasHandler : IRequestHandler<GetInsigniasQuery, IReadOnlyList<InsigniaListItem>>
    {
        private readonly IInsigniaRepository repository;
        public GetInsigniasHandler(IInsigniaRepository _repository) => repository = _repository;

        public async Task<IReadOnlyList<InsigniaListItem>> Handle(GetInsigniasQuery request, CancellationToken ct)
        {
            var insignias = await repository.ListAsync(ct);
            var items = new List<InsigniaListItem>();

            foreach (var insignia in insignias)
            {
                var regionIds = await repository.GetRequiredRegionIdsAsync(insignia.Id, ct);
                var retoIds = await repository.GetRequiredRetoIdsAsync(insignia.Id, ct);

                items.Add(new InsigniaListItem
                {
                    Id = insignia.Id,
                    Name = insignia.Name,
                    Description = insignia.Description,
                    ImageUrl = insignia.ImageUrl,
                    IsActive = insignia.IsActive,
                    MinPoints = insignia.MinPoints,
                    RequiredRegionCount = regionIds.Count,
                    RequiredRetoCount = retoIds.Count,
                    MinRetosCompleted = insignia.MinRetosCompleted,
                    MinPerfectRetos = insignia.MinPerfectRetos,
                    RequireAllQuestionTypes = insignia.RequireAllQuestionTypes,
                    MinLevel = insignia.MinLevel,
                    RequirePremium = insignia.RequirePremium,
                    MinLoginStreakDays = insignia.MinLoginStreakDays,
                    MinAnswerStreak = insignia.MinAnswerStreak,
                    MinRegionsExplored = insignia.MinRegionsExplored,
                    RequiredZoneCode = insignia.RequiredZoneCode,
                    MinZoneRegionsExplored = insignia.MinZoneRegionsExplored,
                    CreatedAt = insignia.CreatedAt
                });
            }

            return items;
        }
    }
}
