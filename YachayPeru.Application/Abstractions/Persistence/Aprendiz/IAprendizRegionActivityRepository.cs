using YachayPeru.Domain.Entities.Aprendiz;

namespace YachayPeru.Application.Abstractions.Persistence.Aprendiz
{
    public interface IAprendizRegionActivityRepository : IRepository<AprendizRegionActivity>
    {
        Task<AprendizRegionActivity?> GetByUserIdAsync(int userId, CancellationToken ct = default);
    }
}
