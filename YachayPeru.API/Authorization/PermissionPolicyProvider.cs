using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace YachayPeru.API.Authorization
{
    /// <summary>
    /// Convierte cualquier nombre de policy no registrado en un PermissionRequirement.
    /// Permite usar [Authorize(Policy = "cursos:create")] sin pre-registrar cada permiso.
    /// </summary>
    public sealed class PermissionPolicyProvider : DefaultAuthorizationPolicyProvider
    {
        public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
            : base(options) { }

        public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            var policy = await base.GetPolicyAsync(policyName);
            if (policy is not null)
                return policy;

            // Cualquier string no registrado se trata como un permiso requerido
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddRequirements(new PermissionRequirement(policyName))
                .Build();
        }
    }
}
