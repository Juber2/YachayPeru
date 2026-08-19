using YachayPeru.Domain.Entities.Aprendiz;

namespace YachayPeru.Application.Abstractions.Persistence.Aprendiz
{
    public interface IAprendizInsigniaEarnedRepository : IRepository<AprendizInsigniaEarned>
    {
        Task<IReadOnlyList<AprendizInsigniaEarned>> GetByUserAsync(int userId, CancellationToken ct = default);
        Task<bool> HasEarnedAsync(int userId, int insigniaId, CancellationToken ct = default);
    }
}
