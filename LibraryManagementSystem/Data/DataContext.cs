using System.Collections.Generic;
using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;



namespace LibraryManagementSystem.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        
        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
           .HasIndex(e => e.Email)
           .IsUnique();

            modelBuilder.Entity<User>()
           .Property(u => u.RoleName)
           .HasDefaultValue("Student");


            modelBuilder.Entity<User>()
                .Property(u => u.Status)
                .HasDefaultValue("Active");
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .Property(u => u.NoofBooks)
                .HasDefaultValue(0);
            base.OnModelCreating(modelBuilder);
        }
    }
}
