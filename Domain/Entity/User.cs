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
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string Role { get; set; } = null!; // Admin, Personel, Vatandaş

        // Hangi departmana ait olduğu
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }

        // İlişkiler
        public ICollection<Complaint>? Complaints { get; set; }
        public ICollection<Assignment>? Assignments { get; set; }
    }
}
