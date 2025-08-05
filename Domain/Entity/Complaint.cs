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
        public int KullaniciId { get; set; }
        public User Kullanici { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public string Aciklama { get; set; }
        public string Durum { get; set; } // Beklemede, Yapılıyor, Tamamlandı
        public DateTime Tarih { get; set; }
        public ICollection<Assignment> Assignments { get; set; }
        public ICollection<ComplaintHistory> History { get; set; }
    }

}
