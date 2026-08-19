using Microsoft.EntityFrameworkCore;
using YachayPeru.Application.Abstractions.Persistence.Assessment;
using YachayPeru.Domain.Entities.Assessment;
using YachayPeru.Infrastructure.Persistence.SqlServer;
using System.Linq.Expressions;

namespace YachayPeru.Infrastructure.Persistence.Repositories
{
    public class RetoVersionQuestionRepository : IRetoVersionQuestionRepository
    {
        private readonly ApplicationDbContext context;

        public RetoVersionQuestionRepository(ApplicationDbContext _context) => context = _context;

        public async Task<RetoVersionQuestion> AddAsync(RetoVersionQuestion entity, CancellationToken ct = default)
        {
            await context.RetoVersionQuestions.AddAsync(entity, ct);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<RetoVersionQuestion> entities, CancellationToken ct = default)
            => await context.RetoVersionQuestions.AddRangeAsync(entities, ct);

        public async Task<bool> AnyAsync(Expression<Func<RetoVersionQuestion, bool>> predicate, CancellationToken ct = default)
            => await context.RetoVersionQuestions.Where(x => !x.Deleted).AnyAsync(predicate, ct);

        public async Task<int> CountAsync(Expression<Func<RetoVersionQuestion, bool>>? predicate = null, CancellationToken ct = default)
        {
            var q = context.RetoVersionQuestions.Where(x => !x.Deleted);
            if (predicate is not null) q = q.Where(predicate);
            return await q.CountAsync(ct);
        }

        public void Delete(RetoVersionQuestion entity) => context.RetoVersionQuestions.Remove(entity);
        public void DeleteRange(IEnumerable<RetoVersionQuestion> entities) => context.RetoVersionQuestions.RemoveRange(entities);

        public async Task<RetoVersionQuestion?> FirstOrDefaultAsync(Expression<Func<RetoVersionQuestion, bool>> predicate, CancellationToken ct = default)
            => await context.RetoVersionQuestions.Where(x => !x.Deleted).FirstOrDefaultAsync(predicate, ct);

        public async Task<RetoVersionQuestion?> GetByIdAsync(object id, CancellationToken ct = default)
        {
            if (id is not int qId) return null;
            return await context.RetoVersionQuestions.FirstOrDefaultAsync(x => x.Id == qId && !x.Deleted, ct);
        }

        public async Task<IReadOnlyList<RetoVersionQuestion>> ListAsync(CancellationToken ct = default)
            => await context.RetoVersionQuestions.Where(x => !x.Deleted).ToListAsync(ct);

        public async Task<IReadOnlyList<RetoVersionQuestion>> ListAsync(Expression<Func<RetoVersionQuestion, bool>> predicate, CancellationToken ct = default)
            => await context.RetoVersionQuestions.Where(x => !x.Deleted).Where(predicate).ToListAsync(ct);

        public async Task<RetoVersionQuestion?> SingleOrDefaultAsync(Expression<Func<RetoVersionQuestion, bool>> predicate, CancellationToken ct = default)
            => await context.RetoVersionQuestions.Where(x => !x.Deleted).SingleOrDefaultAsync(predicate, ct);

        public void Update(RetoVersionQuestion entity) => context.RetoVersionQuestions.Update(entity);

        public async Task<IReadOnlyList<RetoVersionQuestion>> GetByRetoVersionAsync(int retoVersionId, CancellationToken ct = default)
            => await context.RetoVersionQuestions
                .Where(x => x.RetoVersionId == retoVersionId && !x.Deleted)
                .OrderBy(x => x.OrderIndex)
                .ToListAsync(ct);

        public async Task<IReadOnlyList<RetoVersionQuestionChoice>> GetChoicesByQuestionAsync(int questionId, CancellationToken ct = default)
            => await context.RetoVersionQuestionChoices
                .Where(x => x.RetoVersionQuestionId == questionId && !x.Deleted)
                .OrderBy(x => x.OrderIndex)
                .ToListAsync(ct);

        public async Task<int> GetNextOrderIndexAsync(int retoVersionId, CancellationToken ct = default)
        {
            var max = await context.RetoVersionQuestions
                .Where(x => x.RetoVersionId == retoVersionId && !x.Deleted)
                .MaxAsync(x => (int?)x.OrderIndex, ct);
            return (max ?? 0) + 1;
        }

        public async Task DeleteChoicesByQuestionAsync(int questionId, CancellationToken ct = default)
        {
            var choices = await context.RetoVersionQuestionChoices
                .Where(x => x.RetoVersionQuestionId == questionId && !x.Deleted)
                .ToListAsync(ct);
            context.RetoVersionQuestionChoices.RemoveRange(choices);
        }

        public async Task AddChoicesAsync(IEnumerable<RetoVersionQuestionChoice> choices, CancellationToken ct = default)
            => await context.RetoVersionQuestionChoices.AddRangeAsync(choices, ct);

        public async Task<IReadOnlyList<RetoVersionQuestionBlank>> GetBlanksByQuestionAsync(int questionId, CancellationToken ct = default)
            => await context.RetoVersionQuestionBlanks
                .Where(x => x.RetoVersionQuestionId == questionId && !x.Deleted)
                .OrderBy(x => x.OrderIndex)
                .ToListAsync(ct);

        public async Task DeleteBlanksByQuestionAsync(int questionId, CancellationToken ct = default)
        {
            var blanks = await context.RetoVersionQuestionBlanks
                .Where(x => x.RetoVersionQuestionId == questionId && !x.Deleted)
                .ToListAsync(ct);
            context.RetoVersionQuestionBlanks.RemoveRange(blanks);
        }

        public async Task AddBlanksAsync(IEnumerable<RetoVersionQuestionBlank> blanks, CancellationToken ct = default)
            => await context.RetoVersionQuestionBlanks.AddRangeAsync(blanks, ct);

        public async Task<IReadOnlyList<string>> GetDistinctQuestionTypeCodesByRetoVersionIdsAsync(IEnumerable<int> retoVersionIds, CancellationToken ct = default)
            => await context.RetoVersionQuestions
                .Where(x => retoVersionIds.Contains(x.RetoVersionId) && !x.Deleted)
                .Select(x => x.QuestionTypeCode)
                .Distinct()
                .ToListAsync(ct);
    }
}
