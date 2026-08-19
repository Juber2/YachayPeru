using YachayPeru.Domain.Entities.Assessment;

namespace YachayPeru.Application.Abstractions.Persistence.Assessment
{
    public interface IRetoVersionRepository : IRepository<RetoVersion>
    {
        Task<RetoVersion?> GetPublishedByRetoAsync(int retoId, CancellationToken ct = default);
        Task<RetoVersion?> GetDraftByRetoAsync(int retoId, CancellationToken ct = default);
        Task<IReadOnlyList<RetoVersion>> GetHistoryByRetoAsync(int retoId, CancellationToken ct = default);
        Task<int> GetNextVersionNumberAsync(int retoId, CancellationToken ct = default);
    }
}
