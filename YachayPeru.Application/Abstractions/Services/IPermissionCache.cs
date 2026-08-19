namespace YachayPeru.Application.Abstractions.Services
{
    public interface IPermissionCache
    {
        /// <summary>
        /// Devuelve los permisos del rol como strings "resource:action".
        /// Carga desde DB en el primer acceso y cachea el resultado.
        /// </summary>
        Task<IReadOnlyList<string>> GetPermissionsAsync(string roleCode, CancellationToken ct = default);

        /// <summary>Invalida el caché de un rol (usar cuando cambian sus permisos).</summary>
        Task InvalidateAsync(string roleCode, CancellationToken ct = default);
    }
}
