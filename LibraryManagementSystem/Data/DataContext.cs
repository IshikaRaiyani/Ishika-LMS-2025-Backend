using System.Collections.Generic;
using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;



namespace LibraryManagementSystem.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        
        public DbSet<User> Users { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<BookManagement> BookManagement { get; set; }

        public DbSet<Studentwishlist> studentwishlists { get; set; }


        public DbSet<StudentNotification> studentNotifications { get; set; }

       

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

            modelBuilder.Entity<BookManagement>()
           .ToTable(l => l.HasCheckConstraint("CK_BookManagement_RequestType", "RequestType IN ('BorrowRequested','Borrowed','ReturnRequested', 'Returned')"));

            modelBuilder.Entity<BookManagement>()

           .ToTable(l => l.HasCheckConstraint("CK_BookManagement_BorrowStatus", "BorrowStatus IN ('Pending', 'Approved','Rejected')"));

            modelBuilder.Entity<BookManagement>()

          .ToTable(l => l.HasCheckConstraint("CK_BookManagement_ReturnStatus", "ReturnStatus IN ('Pending', 'Approved','Rejected','None')"));
        }
    }
}
