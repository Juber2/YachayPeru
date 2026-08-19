using MediatR;
using YachayPeru.Application.Abstractions.Persistence.Aprendiz;
using YachayPeru.Application.Abstractions.Persistence.Content;

namespace YachayPeru.Application.Features.Aprendiz.Insignias.Queries.GetInsignias
{
    public class GetInsigniasHandler : IRequestHandler<GetInsigniasQuery, IReadOnlyList<AprendizInsigniaListItem>>
    {
        private readonly IInsigniaRepository insigniaRepository;
        private readonly IAprendizInsigniaEarnedRepository earnedRepository;

        public GetInsigniasHandler(IInsigniaRepository _insigniaRepository, IAprendizInsigniaEarnedRepository _earnedRepository)
        {
            insigniaRepository = _insigniaRepository;
            earnedRepository = _earnedRepository;
        }

        public async Task<IReadOnlyList<AprendizInsigniaListItem>> Handle(GetInsigniasQuery request, CancellationToken ct)
        {
            var insignias = await insigniaRepository.ListAsync(x => x.IsActive, ct);
            var earned = await earnedRepository.GetByUserAsync(request.UserId, ct);
            var earnedByInsignia = earned.ToDictionary(e => e.InsigniaId, e => e.EarnedAt);

            return insignias.Select(i => new AprendizInsigniaListItem
            {
                Id = i.Id,
                Name = i.Name,
                Description = i.Description,
                ImageUrl = i.ImageUrl,
                IsEarned = earnedByInsignia.ContainsKey(i.Id),
                EarnedAt = earnedByInsignia.TryGetValue(i.Id, out var earnedAt) ? earnedAt : null
            }).ToList();
        }
    }
}
