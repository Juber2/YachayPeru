using YachayPeru.Domain.Entities.Content;

namespace YachayPeru.Application.Abstractions.Persistence.Content
{
    public interface IInsigniaRepository : IRepository<Insignia>
    {
        Task<IReadOnlyList<int>> GetRequiredRegionIdsAsync(int insigniaId, CancellationToken ct = default);
        Task<IReadOnlyList<int>> GetRequiredRetoIdsAsync(int insigniaId, CancellationToken ct = default);
        Task ReplaceRequiredRegionsAsync(int insigniaId, IReadOnlyCollection<int> regionIds, CancellationToken ct = default);
        Task ReplaceRequiredRetosAsync(int insigniaId, IReadOnlyCollection<int> retoIds, CancellationToken ct = default);
    }
}
