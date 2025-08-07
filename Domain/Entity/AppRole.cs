using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class AppRole
    {
        public int AppRoleId { get; set; }
        public string AppRoleName { get; set; }
        public List<AppUser> Users { get; set; }
    }
}
