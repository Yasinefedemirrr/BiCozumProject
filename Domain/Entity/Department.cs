using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class Department
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<Complaint> Complaints { get; set; }
    }
}
