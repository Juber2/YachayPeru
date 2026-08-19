using Microsoft.EntityFrameworkCore;
using YachayPeru.Application.Abstractions.Persistence.Aprendiz;
using YachayPeru.Domain.Entities.Aprendiz;
using YachayPeru.Infrastructure.Persistence.SqlServer;
using System.Linq.Expressions;

namespace YachayPeru.Infrastructure.Persistence.Repositories
{
    public class FestividadReminderRepository : IFestividadReminderRepository
    {
        private readonly ApplicationDbContext context;

        public FestividadReminderRepository(ApplicationDbContext _context) => context = _context;

        public async Task<FestividadReminder> AddAsync(FestividadReminder entity, CancellationToken ct = default)
        {
            await context.FestividadReminders.AddAsync(entity, ct);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<FestividadReminder> entities, CancellationToken ct = default)
            => await context.FestividadReminders.AddRangeAsync(entities, ct);

        public async Task<bool> AnyAsync(Expression<Func<FestividadReminder, bool>> predicate, CancellationToken ct = default)
            => await context.FestividadReminders.Where(x => !x.Deleted).AnyAsync(predicate, ct);

        public async Task<int> CountAsync(Expression<Func<FestividadReminder, bool>>? predicate = null, CancellationToken ct = default)
        {
            var q = context.FestividadReminders.Where(x => !x.Deleted);
            if (predicate is not null) q = q.Where(predicate);
            return await q.CountAsync(ct);
        }

        public void Delete(FestividadReminder entity) => context.FestividadReminders.Remove(entity);
        public void DeleteRange(IEnumerable<FestividadReminder> entities) => context.FestividadReminders.RemoveRange(entities);

        public async Task<FestividadReminder?> FirstOrDefaultAsync(Expression<Func<FestividadReminder, bool>> predicate, CancellationToken ct = default)
            => await context.FestividadReminders.Where(x => !x.Deleted).FirstOrDefaultAsync(predicate, ct);

        public async Task<FestividadReminder?> GetByIdAsync(object id, CancellationToken ct = default)
        {
            if (id is not int rid) return null;
            return await context.FestividadReminders.FirstOrDefaultAsync(x => x.Id == rid && !x.Deleted, ct);
        }

        public async Task<IReadOnlyList<FestividadReminder>> ListAsync(CancellationToken ct = default)
            => await context.FestividadReminders.Where(x => !x.Deleted).ToListAsync(ct);

        public async Task<IReadOnlyList<FestividadReminder>> ListAsync(Expression<Func<FestividadReminder, bool>> predicate, CancellationToken ct = default)
            => await context.FestividadReminders.Where(x => !x.Deleted).Where(predicate).ToListAsync(ct);

        public async Task<FestividadReminder?> SingleOrDefaultAsync(Expression<Func<FestividadReminder, bool>> predicate, CancellationToken ct = default)
            => await context.FestividadReminders.Where(x => !x.Deleted).SingleOrDefaultAsync(predicate, ct);

        public void Update(FestividadReminder entity) => context.FestividadReminders.Update(entity);

        public async Task<FestividadReminder?> GetByUserAndFestividadAsync(int userId, int festividadId, CancellationToken ct = default)
            => await context.FestividadReminders
                .FirstOrDefaultAsync(x => x.UserId == userId && x.FestividadId == festividadId && !x.Deleted, ct);

        public async Task<IReadOnlyList<FestividadReminder>> GetByUserAsync(int userId, CancellationToken ct = default)
            => await context.FestividadReminders.Where(x => x.UserId == userId && !x.Deleted).ToListAsync(ct);
    }
}
