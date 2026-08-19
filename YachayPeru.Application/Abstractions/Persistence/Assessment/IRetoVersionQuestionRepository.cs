using YachayPeru.Domain.Entities.Assessment;

namespace YachayPeru.Application.Abstractions.Persistence.Assessment
{
    public interface IRetoVersionQuestionRepository : IRepository<RetoVersionQuestion>
    {
        Task<IReadOnlyList<RetoVersionQuestion>> GetByRetoVersionAsync(int retoVersionId, CancellationToken ct = default);
        Task<IReadOnlyList<RetoVersionQuestionChoice>> GetChoicesByQuestionAsync(int questionId, CancellationToken ct = default);
        Task<int> GetNextOrderIndexAsync(int retoVersionId, CancellationToken ct = default);
        Task DeleteChoicesByQuestionAsync(int questionId, CancellationToken ct = default);
        Task AddChoicesAsync(IEnumerable<RetoVersionQuestionChoice> choices, CancellationToken ct = default);
        Task<IReadOnlyList<RetoVersionQuestionBlank>> GetBlanksByQuestionAsync(int questionId, CancellationToken ct = default);
        Task DeleteBlanksByQuestionAsync(int questionId, CancellationToken ct = default);
        Task AddBlanksAsync(IEnumerable<RetoVersionQuestionBlank> blanks, CancellationToken ct = default);
        Task<IReadOnlyList<string>> GetDistinctQuestionTypeCodesByRetoVersionIdsAsync(IEnumerable<int> retoVersionIds, CancellationToken ct = default);
    }
}
