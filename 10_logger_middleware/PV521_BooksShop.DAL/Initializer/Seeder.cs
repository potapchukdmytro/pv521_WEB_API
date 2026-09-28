using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PV521_BooksShop.DAL.Abstraction;
using PV521_BooksShop.DAL.Entities;

namespace PV521_BooksShop.DAL.Initializer
{
    public static class Seeder
    {
        public static void Seed(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
            context.Database.Migrate();
            SeedBooksAuthors(context);
            SeedUsersAndRoles(context, userService);
        }

        private static void SeedUsersAndRoles(AppDbContext context, IUserService userService)
        {
            if(context.Users.Any())
            {
                return;
            }

            var admin = new User
            {
                Email = "admin@mail.com",
                UserName = "admin",
                EmailConfirmed = true,
                FirstName = "John",
                LastName = "Doe"
            };

            var user = new User
            {
                Email = "user@mail.com",
                UserName = "user",
                EmailConfirmed = true,
                FirstName = "Mike",
                LastName = "Thomson"
            };

            userService.CreateAsync(admin, "qwerty").Wait();
            userService.CreateAsync(user, "qwerty").Wait();
        }

        private static void SeedBooksAuthors(AppDbContext context)
        {
            if (context.Authors.Any())
            {
                return;
            }

            var path = Path.Combine(AppContext.BaseDirectory, "Storage", "authors-books.json");
            using var file = File.OpenRead(path);
            var authors = JsonSerializer.Deserialize<List<Author>>(file)
                ?? throw new InvalidDataException($"Seed file '{path}' must contain an array of authors.");

            if (authors.Count == 0 || authors.Any(author =>
                string.IsNullOrWhiteSpace(author.Name) || author.Books.Count == 0 ||
                author.Books.Any(book => string.IsNullOrWhiteSpace(book.Title))))
            {
                throw new InvalidDataException($"Seed file '{path}' contains missing authors or books.");
            }

            // EF Core inserts the nested books and assigns their AuthorId values.
            context.Authors.AddRange(authors);
            context.SaveChanges();
        }
    }
}
