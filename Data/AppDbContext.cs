using DoroTech.Bookstore.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace DoroTech.Bookstore.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Book> Books => Set<Book>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasIndex(b => b.Title)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
