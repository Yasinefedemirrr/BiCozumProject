using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Context
{
    public class BiCozumContextFactory : IDesignTimeDbContextFactory<BiCozumContext>
    {
        public BiCozumContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<BiCozumContext>();

            // Connection string
            optionsBuilder.UseSqlServer("Server=YASINEFEDEMIR\\SQLEXPRESS;Database=BiCozum;Trusted_Connection=True;TrustServerCertificate=True;");

            return new BiCozumContext(optionsBuilder.Options);
        }
    }
}
