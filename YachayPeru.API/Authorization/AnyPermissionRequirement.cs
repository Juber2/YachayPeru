using Microsoft.AspNetCore.Authorization;
using YachayPeru.Application.Abstractions.Services;

namespace YachayPeru.API.Authorization
{
    public sealed class AnyPermissionRequirement : IAuthorizationRequirement
    {
        public IReadOnlyList<string> Permissions { get; }

        public AnyPermissionRequirement(params string[] permissions)
        {
            Permissions = permissions;
        }
    }

    public sealed class AnyPermissionAuthorizationHandler
        : AuthorizationHandler<AnyPermissionRequirement>
    {
        private readonly IPermissionCache _permissionCache;

        public AnyPermissionAuthorizationHandler(IPermissionCache permissionCache)
        {
            _permissionCache = permissionCache;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            AnyPermissionRequirement requirement)
        {
            var tokenPermissions = context.User
                .FindAll("permission")
                .Select(c => c.Value)
                .ToHashSet();

            if (requirement.Permissions.Any(p => tokenPermissions.Contains(p)))
            {
                context.Succeed(requirement);
                return;
            }

            var roleCodes = context.User.FindAll("roles").Select(c => c.Value);
            foreach (var roleCode in roleCodes)
            {
                var rolePermissions = await _permissionCache.GetPermissionsAsync(roleCode);
                if (requirement.Permissions.Any(p => rolePermissions.Contains(p)))
                {
                    context.Succeed(requirement);
                    return;
                }
            }
        }
    }
}
