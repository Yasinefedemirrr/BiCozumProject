using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class Assignment
    {
        public int Id { get; set; }
        public DateTime AssignedAt { get; set; } = DateTime.Now;
        public string Progress { get; set; } = "Atandı"; // Atandı, Test Ediliyor, Tamamlandı

        // Şikayet
        public int ComplaintId { get; set; }
        public Complaint Complaint { get; set; }

        // Atanan personel
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
