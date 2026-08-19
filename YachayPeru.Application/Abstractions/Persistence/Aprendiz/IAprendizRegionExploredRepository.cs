using YachayPeru.Domain.Entities.Aprendiz;

namespace YachayPeru.Application.Abstractions.Persistence.Aprendiz
{
    public interface IAprendizRegionExploredRepository : IRepository<AprendizRegionExplored>
    {
        Task<bool> HasExploredAsync(int userId, int regionId, CancellationToken ct = default);
        Task<IReadOnlyList<int>> GetExploredRegionIdsAsync(int userId, CancellationToken ct = default);
    }
}
