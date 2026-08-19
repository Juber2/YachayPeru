using YachayPeru.Domain.Entities.Aprendiz;

namespace YachayPeru.Application.Abstractions.Persistence.Aprendiz
{
    public interface IAprendizProfileRepository : IRepository<AprendizProfile>
    {
        Task<AprendizProfile?> GetByUserIdAsync(int userId, CancellationToken ct = default);
    }
}
