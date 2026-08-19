using YachayPeru.Domain.Entities.Learning;

namespace YachayPeru.Application.Abstractions.Persistence.Learning
{
    public interface ICourseVersionRepository : IRepository<CourseVersion>
    {
        Task<CourseVersion?> GetCurrentAsync(int courseId, CancellationToken ct = default);
        Task<CourseVersion?> GetDraftAsync(int courseId, CancellationToken ct = default);
        Task<IReadOnlyList<CourseVersion>> GetHistoryAsync(int courseId, CancellationToken ct = default);
        Task<int> GetNextVersionNumberAsync(int courseId, CancellationToken ct = default);
    }
}
