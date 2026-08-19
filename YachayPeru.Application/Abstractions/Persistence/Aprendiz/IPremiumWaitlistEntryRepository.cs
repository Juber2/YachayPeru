using YachayPeru.Domain.Entities.Aprendiz;

namespace YachayPeru.Application.Abstractions.Persistence.Aprendiz
{
    public interface IPremiumWaitlistEntryRepository : IRepository<PremiumWaitlistEntry>
    {
        Task<PremiumWaitlistEntry?> GetByUserIdAsync(int userId, CancellationToken ct = default);
    }
}
