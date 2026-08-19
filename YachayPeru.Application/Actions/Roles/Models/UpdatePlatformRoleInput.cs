namespace YachayPeru.Application.Actions.Roles.Models
{
    public class UpdatePlatformRoleInput
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string RoleCode { get; set; } = string.Empty;
        public string? Description { get; set; }
        public IReadOnlyCollection<int> PermissionIds { get; set; } = [];
    }
}
