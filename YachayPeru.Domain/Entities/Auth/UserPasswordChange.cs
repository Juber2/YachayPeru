using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YachayPeru.Domain.Entities.Auth
{
    public class UserPasswordChange
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Password { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public User User { get; set; } = null!;
    }
}
