namespace YachayPeru.API.Contracts.Administration.Roles.Request
{
    public class CreateRoleRequest
    {
        public string Name { get; set; } = default!;
        public string? RoleCode { get; set; } = default!;
        public string? Description { get; set; }
        public int[] PermissionIds { get; set; } = [];
    }
}
