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
        public int ComplaintId { get; set; }
        public Complaint Complaint { get; set; }
        public string Aciklama { get; set; }
        public DateTime Tarih { get; set; }
    }
}
