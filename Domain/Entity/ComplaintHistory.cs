using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class ComplaintHistory
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string Status { get; set; } = null!;
        public string? Note { get; set; }

        // Şikayet
        public int ComplaintId { get; set; }
        public Complaint Complaint { get; set; }
    }
}
