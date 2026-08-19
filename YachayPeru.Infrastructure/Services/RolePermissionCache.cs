using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using YachayPeru.Application.Abstractions.Services;
using YachayPeru.Infrastructure.Persistence.SqlServer;

namespace YachayPeru.Infrastructure.Services
{
    /// <summary>
    /// Caché de permisos por rol usando IMemoryCache.
    /// Es singleton — usa IServiceScopeFactory para acceder al DbContext (scoped).
    /// TTL: 5 minutos. Llamar a InvalidateAsync cuando cambien los permisos de un rol.
    /// </summary>
    public sealed class RolePermissionCache : IPermissionCache
    {
        private readonly IMemoryCache _cache;
        private readonly IServiceScopeFactory _scopeFactory;
        private static readonly TimeSpan CacheTtl = TimeSpan.FromMinutes(5);

        public RolePermissionCache(IMemoryCache cache, IServiceScopeFactory scopeFactory)
        {
            _cache = cache;
            _scopeFactory = scopeFactory;
        }

        public async Task<IReadOnlyList<string>> GetPermissionsAsync(string roleCode, CancellationToken ct = default)
        {
            var key = CacheKey(roleCode);

            if (_cache.TryGetValue(key, out IReadOnlyList<string>? cached))
                return cached!;

            var permissions = await LoadFromDbAsync(roleCode, ct);
            _cache.Set(key, permissions, CacheTtl);
            return permissions;
        }

        public Task InvalidateAsync(string roleCode, CancellationToken ct = default)
        {
            _cache.Remove(CacheKey(roleCode));
            return Task.CompletedTask;
        }

        private async Task<IReadOnlyList<string>> LoadFromDbAsync(string roleCode, CancellationToken ct)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var permissions = await (
                from rp in context.PlatformRoles
                where rp.RoleCode == roleCode && !rp.Deleted
                join rpp in context.PlatformRolePermissions on rp.Id equals rpp.PlatformRoleId
                where !rpp.Deleted
                join p in context.Permissions on rpp.PermissionId equals p.Id
                where !p.Deleted
                join r in context.Resources on p.ResourceId equals r.Id
                join mc in context.MasterCodes on p.PermissionCode equals mc.Code
                select $"{r.Code}:{mc.Name}"
            ).Distinct().ToListAsync(ct);

            return permissions;
        }

        private static string CacheKey(string roleCode) => $"role_permissions:{roleCode}";
    }
}
