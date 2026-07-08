using DoroTech.Bookstore.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace DoroTech.Bookstore.Api.Data
{
    public class SeedData
    {
        public static async Task InitializeAsync(AppDbContext context)
        {
            await context.Database.MigrateAsync();

            if (!await context.Users.AnyAsync())
            {
                context.Users.Add(new User
                {
                    Username = "admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                    Role = "Admin"
                });
            }

            if (!await context.Books.AnyAsync())
            {
                context.Books.AddRange(
                    new Book
                    {
                        Title = "Clean Code",
                        Author = "Robert C. Martin",
                        Isbn = "9780132350884",
                        PublicationYear = 2008,
                        Quantity = 5
                    },
                    new Book
                    {
                        Title = "Domain-Driven Design",
                        Author = "Eric Evans",
                        Isbn = "9780321125217",
                        PublicationYear = 2003,
                        Quantity = 3
                    },
                    new Book
                    {
                        Title = "Refactoring",
                        Author = "Martin Fowler",
                        Isbn = "9780201485677",
                        PublicationYear = 1999,
                        Quantity = 4
                    }
                );
            }

            await context.SaveChangesAsync();
        }
    }
}
