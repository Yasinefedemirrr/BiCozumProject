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

            // User -> Department (Restrict - manuel silme yapılacak)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Department)
                .WithMany(d => d.Users)
                .HasForeignKey(u => u.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Complaint -> User (Cascade)
            modelBuilder.Entity<Complaint>()
                .HasOne(c => c.User)
                .WithMany(u => u.Complaints)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Complaint -> Department (Cascade)
            modelBuilder.Entity<Complaint>()
                .HasOne(c => c.Department)
                .WithMany(d => d.Complaints)
                .HasForeignKey(c => c.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Assignment -> Complaint (Cascade)
            modelBuilder.Entity<Assignment>()
                .HasOne(a => a.Complaint)
                .WithMany(c => c.Assignments)
                .HasForeignKey(a => a.ComplaintId)
                .OnDelete(DeleteBehavior.Cascade);

            // Assignment -> User (Restrict)
            modelBuilder.Entity<Assignment>()
                .HasOne(a => a.User)
                .WithMany(u => u.Assignments)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // ComplaintHistory -> Complaint (Cascade)
            modelBuilder.Entity<ComplaintHistory>()
                .HasOne(ch => ch.Complaint)
                .WithMany(c => c.ComplaintHistories)
                .HasForeignKey(ch => ch.ComplaintId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
