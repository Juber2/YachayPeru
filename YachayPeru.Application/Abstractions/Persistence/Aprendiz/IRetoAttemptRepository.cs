using YachayPeru.Domain.Entities.Aprendiz;

namespace YachayPeru.Application.Abstractions.Persistence.Aprendiz
{
    public interface IRetoAttemptRepository : IRepository<RetoAttempt>
    {
        Task<int> CountByUserAndRetoAsync(int userId, int retoId, CancellationToken ct = default);
        Task<bool> HasPassedAsync(int userId, int retoId, CancellationToken ct = default);
        Task<IReadOnlyList<int>> GetPassedRetoIdsByUserAsync(int userId, CancellationToken ct = default);
        Task<RetoAttempt?> GetBestByUserAndRetoAsync(int userId, int retoId, CancellationToken ct = default);
        Task<int> CountDistinctPerfectRetosByUserAsync(int userId, CancellationToken ct = default);
    }
}
