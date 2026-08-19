using YachayPeru.Domain.Base;
using YachayPeru.Domain.Entities.Common;

namespace YachayPeru.Domain.Entities.Access
{
    public class Permission : BaseEntity
    {
        public int ResourceId { get; set; }
        public string PermissionCode { get; set; } = string.Empty;
        public Resource Resource { get; set; } = null!;
        public MasterCode PermissionMasterCode { get; set; } = null!;
    }
}
