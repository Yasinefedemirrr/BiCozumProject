using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class User
    {
        public int Id { get; set; }
        public string AdSoyad { get; set; }
        public string Email { get; set; }
        public string SifreHash { get; set; }
        public string Rol { get; set; } // User, Staff, Admin
        public int? DepartmentId { get; set; }
        public Department Department { get; set; }
    }
}
