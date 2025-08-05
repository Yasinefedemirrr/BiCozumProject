using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Persistance.Context
{
    public class BiCozumContext : DbContext
    {
        public BiCozumContext(DbContextOptions<BiCozumContext> options) : base(options)
        {
        }

        // DbSet tanımlamaları
        public DbSet<User> Users { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Complaint> Complaints { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<ComplaintHistory> ComplaintHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // İleride Fluent API ayarlarını buraya ekleyeceğiz
        }
    }
}
