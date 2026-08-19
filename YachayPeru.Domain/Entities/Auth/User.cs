using YachayPeru.Domain.Base;
using YachayPeru.Domain.Entities.Common;

namespace YachayPeru.Domain.Entities.Auth
{
    public class User : BaseEntity
    {
        public int PersonId { get; set; }
        public string UserTypeCode { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTime? LockedUntil { get; set; }
        public bool IsLocked { get; set; }
        public string? LockedReason { get; set; }
        public string? Email { get; set; }
        public int? RoleId { get; set; }
        public MasterCode UserType { get; set; } = null!;
        public Person Person { get; set; } = null!;
    }
}
