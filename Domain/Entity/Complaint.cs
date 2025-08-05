using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class Complaint
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Status { get; set; } = "Beklemede"; // Beklemede, Atandı, Tamamlandı

        // Vatandaş
        public int UserId { get; set; }
        public User User { get; set; }

        // Hangi müdürlüğe ait
        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        // İlişkiler
        public ICollection<Assignment>? Assignments { get; set; }
        public ICollection<ComplaintHistory>? ComplaintHistories { get; set; }
    }

}
