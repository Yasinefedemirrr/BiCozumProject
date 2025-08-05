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

            // User -> Department (1-N)
            modelBuilder.Entity<User>()
                .HasOne(u => u.Department)
                .WithMany(d => d.Users)
                .HasForeignKey(u => u.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Complaint -> User (1-N, Vatandaş)
            modelBuilder.Entity<Complaint>()
                .HasOne(c => c.User)
                .WithMany(u => u.Complaints)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Complaint -> Department (1-N)
            modelBuilder.Entity<Complaint>()
                .HasOne(c => c.Department)
                .WithMany(d => d.Complaints)
                .HasForeignKey(c => c.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Assignment -> Complaint (1-N) - Cascade kapalı
            modelBuilder.Entity<Assignment>()
                .HasOne(a => a.Complaint)
                .WithMany(c => c.Assignments)
                .HasForeignKey(a => a.ComplaintId)
                .OnDelete(DeleteBehavior.Restrict);

            // Assignment -> User (1-N, Personel) - Cascade kapalı
            modelBuilder.Entity<Assignment>()
                .HasOne(a => a.User)
                .WithMany(u => u.Assignments)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // ComplaintHistory -> Complaint (1-N) - Cascade kapalı
            modelBuilder.Entity<ComplaintHistory>()
                .HasOne(ch => ch.Complaint)
                .WithMany(c => c.ComplaintHistories)
                .HasForeignKey(ch => ch.ComplaintId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
