using YachayPeru.Domain.Entities.Learning;

namespace YachayPeru.Application.Abstractions.Persistence.Learning
{
    public interface IModuleContentFileRepository : IRepository<ModuleContentFile>
    {
        Task<IReadOnlyList<ModuleContentFile>> GetByItemAsync(int moduleContentId, CancellationToken ct = default);
    }
}
