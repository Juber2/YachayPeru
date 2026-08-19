namespace YachayPeru.Application.Abstractions.Persistence.Auth
{
    public sealed record UserRoleAccess
    {
        public int RoleId { get; init; }
        public string RoleCode { get; init; } = default!;
        public string RoleName { get; init; } = default!;
        public IReadOnlyList<UserPermissionEntry> Permissions { get; init; } = [];
    }

    public sealed record UserPermissionEntry
    {
        public string Resource { get; init; } = default!;
        public string Action { get; init; } = default!;
    }
}
