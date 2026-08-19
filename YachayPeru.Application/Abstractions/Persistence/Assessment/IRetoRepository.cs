using YachayPeru.Domain.Entities.Assessment;

namespace YachayPeru.Application.Abstractions.Persistence.Assessment
{
    public record RetoWithCourseRow(int RetoId, int CourseId, string CourseTitle, DateTime CreatedAt);

    public interface IRetoRepository : IRepository<Reto>
    {
        Task<IReadOnlyList<Reto>> GetByCourseAsync(int courseId, CancellationToken ct = default);
        Task<IReadOnlyList<RetoWithCourseRow>> GetAllWithCourseAsync(CancellationToken ct = default);
    }
}
