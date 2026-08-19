using Microsoft.EntityFrameworkCore;
using YachayPeru.Application.Abstractions.Persistence.Auth;
using YachayPeru.Domain.Constants;
using YachayPeru.Domain.Entities.Auth;
using YachayPeru.Infrastructure.Persistence.SqlServer;
using System.Linq.Expressions;

namespace YachayPeru.Infrastructure.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        ApplicationDbContext context;
        public UserRepository(ApplicationDbContext _context) {
            context = _context;
        }

        public async Task<User> AddAsync(User entity, CancellationToken cancellationToken = default)
        {
            await context.Users.AddAsync(entity, cancellationToken);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<User> entities, CancellationToken cancellationToken = default)
        {
            await context.Users.AddRangeAsync(entities, cancellationToken);
        }

        public async Task<bool> AnyAsync(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await context.Users
                .Where(x => !x.Deleted)
                .AnyAsync(predicate, cancellationToken);
        }

        public async Task<int> CountAsync(Expression<Func<User, bool>>? predicate = null, CancellationToken cancellationToken = default)
        {
            IQueryable<User> query = context.Users.Where(x => !x.Deleted);

            if (predicate is not null)
                query = query.Where(predicate);

            return await query.CountAsync(cancellationToken);
        }

        public void Delete(User entity) => context.Users.Remove(entity);

        public void DeleteRange(IEnumerable<User> entities) => context.Users.RemoveRange(entities);

        public async Task<User?> FirstOrDefaultAsync(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await context.Users
                .Where(x => !x.Deleted)
                .FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public async Task<User?> GetByIdAsync(object id, CancellationToken cancellationToken = default)
        {
            if (id is not int userId) return null;

            return await context.Users
                .FirstOrDefaultAsync(x => x.Id == userId && !x.Deleted, cancellationToken);
        }

        public async Task<IReadOnlyList<User>> ListAsync(CancellationToken cancellationToken = default)
        {
            return await context.Users
                .Where(x => !x.Deleted)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<User>> ListAsync(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await context.Users
                .Where(x => !x.Deleted)
                .Where(predicate)
                .ToListAsync(cancellationToken);
        }

        public async Task<User?> SingleOrDefaultAsync(Expression<Func<User, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await context.Users
                .Where(x => !x.Deleted)
                .Where(predicate)
                .SingleOrDefaultAsync(cancellationToken);
        }

        public void Update(User entity) => context.Users.Update(entity);

        public async Task<User?> GetByUsernameWithDetailsAsync(string username, CancellationToken ct = default)
        {
            return await context.Users
                .Include(u => u.Person)
                .Where(x => !x.Deleted)
                .SingleOrDefaultAsync(x => x.Username == username, ct);
        }

        public async Task<DeletedUserInfo?> FindDeletedByUsernameOrEmailAsync(string username, string? email, CancellationToken ct = default)
        {
            return await (
                from u in context.Users.IgnoreQueryFilters()
                where u.Deleted && (u.Username == username || (!string.IsNullOrEmpty(email) && u.Email == email))
                join p in context.Persons.IgnoreQueryFilters() on u.PersonId equals p.Id
                select new DeletedUserInfo(u.Id, p.FirstName + " " + p.LastName, u.UpdatedAt)
            ).FirstOrDefaultAsync(ct);
        }

        public async Task<User?> GetDeletedByIdWithPersonAsync(int id, CancellationToken ct = default)
        {
            return await context.Users
                .IgnoreQueryFilters()
                .Include(u => u.Person)
                .FirstOrDefaultAsync(x => x.Id == id && x.Deleted, ct);
        }

        public async Task<User?> GetByIdWithPersonAsync(int id, CancellationToken ct = default)
        {
            return await context.Users
                .Include(u => u.Person)
                .FirstOrDefaultAsync(x => x.Id == id && !x.Deleted, ct);
        }

        public async Task<IReadOnlyList<UserRoleAccess>> GetUserAccessAsync(int userId, CancellationToken ct = default)
        {
            var user = await context.Users
                .Where(u => u.Id == userId && !u.Deleted)
                .Select(u => new { u.Id, u.RoleId })
                .FirstOrDefaultAsync(ct);

            if (user is null || user.RoleId is null) return [];

            var result = await (
                from rp in context.PlatformRoles
                where rp.Id == user.RoleId && !rp.Deleted
                join rpp in context.PlatformRolePermissions on rp.Id equals rpp.PlatformRoleId into rppLeft
                from rpp in rppLeft.DefaultIfEmpty()
                where rpp == null || !rpp.Deleted
                join p in context.Permissions on rpp!.PermissionId equals p.Id into pLeft
                from p in pLeft.DefaultIfEmpty()
                where p == null || !p.Deleted
                join r in context.Resources on p!.ResourceId equals r.Id into rLeft
                from r in rLeft.DefaultIfEmpty()
                join mc in context.MasterCodes on p!.PermissionCode equals mc.Code into mcLeft
                from mc in mcLeft.DefaultIfEmpty()
                select new
                {
                    RoleId   = rp.Id,
                    RoleCode = (string?)(rp.RoleCode ?? string.Empty),
                    RoleName = rp.Name,
                    Resource = r  != null ? r.Code  : null,
                    Action   = mc != null ? mc.Code : null
                }
            ).ToListAsync(ct);

            return result
                .GroupBy(x => new { x.RoleId, x.RoleCode, x.RoleName })
                .Select(g => new UserRoleAccess
                {
                    RoleId   = g.Key.RoleId,
                    RoleCode = g.Key.RoleCode,
                    RoleName = g.Key.RoleName,
                    Permissions = g.Select(x => new UserPermissionEntry
                    {
                        Resource = x.Resource,
                        Action   = x.Action
                    }).ToList()
                })
                .ToList();
        }

        public async Task<PlatformUserDetailRow?> GetPlatformUserDetailAsync(int userId, CancellationToken ct = default)
        {
            var user = await (
                from u in context.Users.IgnoreQueryFilters()
                where u.Id == userId
                join p in context.Persons.IgnoreQueryFilters() on u.PersonId equals p.Id
                join role in context.PlatformRoles.IgnoreQueryFilters()
                    on u.RoleId equals role.Id into roles
                from role in roles.DefaultIfEmpty()
                select new
                {
                    u.Id, p.FirstName, p.LastName, u.Email, u.Username, u.IsLocked,
                    RoleId   = role != null ? (int?)role.Id : null,
                    RoleName = role != null ? role.Name : null,
                }
            ).FirstOrDefaultAsync(ct);

            if (user is null) return null;

            return new PlatformUserDetailRow(
                user.Id, user.FirstName, user.LastName, user.Email, user.Username,
                user.IsLocked, user.RoleId, user.RoleName);
        }

        public async Task<IReadOnlyList<PlatformUserListRow>> GetPlatformUserListAsync(CancellationToken ct = default)
        {
            var rows = await (
                from u in context.Users
                where !u.Deleted && u.UserTypeCode == AppConstants.UserType.Platform
                join p in context.Persons on u.PersonId equals p.Id
                join role in context.PlatformRoles.Where(x => !x.Deleted)
                    on u.RoleId equals role.Id into roles
                from role in roles.DefaultIfEmpty()
                let lastAccess = context.RefreshTokens
                    .Where(rt => rt.UserId == u.Id)
                    .OrderByDescending(rt => rt.CreatedAt)
                    .Select(rt => (DateTime?)rt.CreatedAt)
                    .FirstOrDefault()
                orderby p.LastName, p.FirstName
                select new PlatformUserListRow(
                    u.Id,
                    p.FirstName,
                    p.LastName,
                    u.Email,
                    u.IsLocked,
                    role != null ? role.Name : null,
                    role != null ? role.RoleCode : null,
                    lastAccess)
            ).ToListAsync(ct);

            return rows;
        }
    }
}
