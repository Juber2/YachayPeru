using Microsoft.EntityFrameworkCore;
using YachayPeru.Application.Abstractions.Persistence.Auth;
using YachayPeru.Domain.Entities.Auth;
using YachayPeru.Infrastructure.Persistence.SqlServer;

namespace YachayPeru.Infrastructure.Persistence.Repositories
{
    public sealed class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly ApplicationDbContext _context;

        public RefreshTokenRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken ct = default)
        {
            return await _context.RefreshTokens
                .FirstOrDefaultAsync(x => x.Token == token, ct);
        }

        public async Task<RefreshToken?> GetByApprovalTokenAsync(string approvalToken, CancellationToken ct = default)
        {
            return await _context.RefreshTokens
                .FirstOrDefaultAsync(x => x.ApprovalToken == approvalToken, ct);
        }

        public async Task AddAsync(RefreshToken refreshToken, CancellationToken ct = default)
        {
            await _context.RefreshTokens.AddAsync(refreshToken, ct);
        }

        public void Update(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Update(refreshToken);
        }

        public void Revoke(RefreshToken refreshToken)
        {
            refreshToken.IsRevoked = true;
            _context.RefreshTokens.Update(refreshToken);
        }

        public async Task RevokeAllByUserIdAsync(int userId, CancellationToken ct = default)
        {
            await _context.RefreshTokens
                .Where(x => x.UserId == userId && !x.IsRevoked)
                .ExecuteUpdateAsync(s => s.SetProperty(x => x.IsRevoked, true), ct);
        }
    }
}
