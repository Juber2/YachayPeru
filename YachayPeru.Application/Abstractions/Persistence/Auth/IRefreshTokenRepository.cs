using YachayPeru.Domain.Entities.Auth;

namespace YachayPeru.Application.Abstractions.Persistence.Auth
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken ct = default);
        Task<RefreshToken?> GetByApprovalTokenAsync(string approvalToken, CancellationToken ct = default);
        Task AddAsync(RefreshToken refreshToken, CancellationToken ct = default);
        void Update(RefreshToken refreshToken);
        void Revoke(RefreshToken refreshToken);
        Task RevokeAllByUserIdAsync(int userId, CancellationToken ct = default);
    }
}
