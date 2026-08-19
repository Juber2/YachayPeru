using YachayPeru.Application.Abstractions.Persistence;
using YachayPeru.Domain.Entities.Auth;

namespace YachayPeru.Application.Abstractions.Persistence.Auth
{
    public interface IUserRepository : IRepository<User>
    {
        /// <summary>Carga User + Person — usar en Login. UserTypeCode está directamente en User.</summary>
        Task<User?> GetByUsernameWithDetailsAsync(string username, CancellationToken ct = default);

        Task<DeletedUserInfo?> FindDeletedByUsernameOrEmailAsync(string username, string? email, CancellationToken ct = default);

        Task<IReadOnlyList<PlatformUserListRow>> GetPlatformUserListAsync(CancellationToken ct = default);
        Task<PlatformUserDetailRow?> GetPlatformUserDetailAsync(int userId, CancellationToken ct = default);

        Task<User?> GetByIdWithPersonAsync(int id, CancellationToken ct = default);
        Task<User?> GetDeletedByIdWithPersonAsync(int id, CancellationToken ct = default);

        Task<IReadOnlyList<UserRoleAccess>> GetUserAccessAsync(int userId, CancellationToken ct = default);
    }
}
