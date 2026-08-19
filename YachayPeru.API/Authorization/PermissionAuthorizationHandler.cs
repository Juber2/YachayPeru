using Microsoft.AspNetCore.Authorization;
using YachayPeru.Application.Abstractions.Services;
using System.Security.Claims;

namespace YachayPeru.API.Authorization
{
    public sealed class PermissionAuthorizationHandler
        : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IPermissionCache _permissionCache;

        public PermissionAuthorizationHandler(IPermissionCache permissionCache)
        {
            _permissionCache = permissionCache;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            // 1. Verificar directo en los claims del token (sin DB/caché)
            var tokenPermissions = context.User
                .FindAll("permission")
                .Select(c => c.Value)
                .ToHashSet();

            if (tokenPermissions.Contains(requirement.Permission))
            {
                context.Succeed(requirement);
                return;
            }

            // 2. Fallback: verificar via caché de roles (para tokens sin permission claims)
            var roleCodes = context.User
                .FindAll("roles")
                .Select(c => c.Value);

            foreach (var roleCode in roleCodes)
            {
                var rolePermissions = await _permissionCache.GetPermissionsAsync(roleCode);
                if (rolePermissions.Contains(requirement.Permission))
                {
                    context.Succeed(requirement);
                    return;
                }
            }
        }
    }
}
