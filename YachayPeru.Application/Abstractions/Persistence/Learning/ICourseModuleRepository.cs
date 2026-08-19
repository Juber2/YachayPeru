using YachayPeru.Domain.Entities.Learning;

namespace YachayPeru.Application.Abstractions.Persistence.Learning
{
    public interface ICourseModuleRepository : IRepository<CourseModule>
    {
        Task<IReadOnlyList<CourseModule>> GetByVersionAsync(int courseVersionId, CancellationToken ct = default);
        Task<int> GetNextOrderIndexAsync(int courseVersionId, CancellationToken ct = default);
    }
}
