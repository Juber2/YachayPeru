using YachayPeru.Domain.Base;
using YachayPeru.Domain.Entities.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YachayPeru.Domain.Entities.Common
{
    public class Person : BaseEntity
    {
        public string? FirstName { get; set; } 
        public string? LastName { get; set; }
        public string? DocumentType { get; set; } 
        public string? DocumentNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? BusinessName { get; set; } 
        public string? Ruc { get; set; }
        public string? LegalRepresentative { get; set; }
        public string? Phone { get; set; } 
        public string? Address { get; set; } 
        public string? City { get; set; } 

    }
}
