using YachayPeru.Domain.Entities.Aprendiz;

namespace YachayPeru.Application.Abstractions.Persistence.Aprendiz
{
    public interface IAprendizActivityLogRepository : IRepository<AprendizActivityLog>
    {
        Task<IReadOnlyList<AprendizActivityLog>> GetByUserAsync(int userId, CancellationToken ct = default);
        Task<AprendizActivityLog?> GetLastWithRegionAsync(int userId, CancellationToken ct = default);
    }
}
