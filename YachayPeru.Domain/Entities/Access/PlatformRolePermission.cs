using YachayPeru.Domain.Base;

namespace YachayPeru.Domain.Entities.Access
{
    public class PlatformRolePermission : BaseEntity
    {
        public int PlatformRoleId { get; set; }
        public int PermissionId { get; set; }
        public PlatformRole PlatformRole { get; set; } = null!;
        public Permission Permission { get; set; } = null!;
    }
}
