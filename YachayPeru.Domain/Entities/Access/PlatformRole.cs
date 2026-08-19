using YachayPeru.Domain.Base;
using YachayPeru.Domain.Entities.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YachayPeru.Domain.Entities.Access
{
    public class PlatformRole : BaseEntity
    {
        public string? RoleCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
