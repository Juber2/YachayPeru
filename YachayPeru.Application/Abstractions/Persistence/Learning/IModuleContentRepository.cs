using YachayPeru.Domain.Entities.Learning;

namespace YachayPeru.Application.Abstractions.Persistence.Learning
{
    public interface IModuleContentRepository : IRepository<ModuleContent>
    {
        Task<IReadOnlyList<ModuleContent>> GetByModuleAsync(int moduleId, CancellationToken ct = default);
        Task<int> GetNextOrderIndexAsync(int moduleId, CancellationToken ct = default);
    }
}
