using YachayPeru.Application.Features.MasterCodes.Queries.GetMasterCodesByParent;

namespace YachayPeru.Application.Abstractions.Persistence.Common
{
    public interface IMasterCodeRepository
    {
        Task<IReadOnlyList<MasterCodeItem>> GetByParentCodeAsync(string parentCode, CancellationToken ct = default);
    }
}
