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
        public int ComplaintId { get; set; }
        public Complaint Complaint { get; set; }
        public int PersonelId { get; set; }
        public User Personel { get; set; }
        public string Durum { get; set; } // Beklemede, Yapılıyor, Test Ediliyor, Tamamlandı
        public DateTime Tarih { get; set; }
    }
}
