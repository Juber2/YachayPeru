using YachayPeru.Domain.Entities.Aprendiz;

namespace YachayPeru.Application.Abstractions.Persistence.Aprendiz
{
    public interface IFestividadReminderRepository : IRepository<FestividadReminder>
    {
        Task<FestividadReminder?> GetByUserAndFestividadAsync(int userId, int festividadId, CancellationToken ct = default);
        Task<IReadOnlyList<FestividadReminder>> GetByUserAsync(int userId, CancellationToken ct = default);
    }
}
