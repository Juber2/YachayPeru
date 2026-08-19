using YachayPeru.Domain.Entities.Access;

namespace YachayPeru.Application.Abstractions.Persistence.Access
{
    public interface IPermissionRepository
    {
        Task<IReadOnlyList<(Resource Resource, Permission Permission, string ActionName)>> GetMatrixAsync(CancellationToken ct = default);
        Task<IReadOnlyList<(Resource Resource, Permission Permission, string ActionName)>> GetByScopeAsync(string scope, CancellationToken ct = default);
        Task<bool> AllExistAsync(IEnumerable<int> ids, CancellationToken ct = default);
        Task<IReadOnlyList<int>> GetIdsByResourceCodesAsync(IEnumerable<string> resourceCodes, CancellationToken ct = default);
    }
}
