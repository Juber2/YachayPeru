using Microsoft.EntityFrameworkCore;
using YachayPeru.Application.Abstractions.Persistence.Common;
using YachayPeru.Application.Features.MasterCodes.Queries.GetMasterCodesByParent;
using YachayPeru.Infrastructure.Persistence.SqlServer;

namespace YachayPeru.Infrastructure.Persistence.Repositories
{
    public class MasterCodeRepository : IMasterCodeRepository
    {
        private readonly ApplicationDbContext _context;

        public MasterCodeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<MasterCodeItem>> GetByParentCodeAsync(string parentCode, CancellationToken ct = default)
        {
            return await _context.MasterCodes
                .Where(x => x.ParentCode == parentCode && x.IsActive)
                .OrderBy(x => x.OrderIndex)
                .Select(x => new MasterCodeItem(x.Code, x.Value))
                .ToListAsync(ct);
        }
    }
}
