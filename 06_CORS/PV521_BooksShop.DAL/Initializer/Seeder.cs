using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PV521_BooksShop.DAL.Entities;

namespace PV521_BooksShop.DAL.Initializer
{
    public static class Seeder
    {
        public static void Seed(this IApplicationBuilder app)
        {
            var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            context.Database.Migrate();

            SeedBooksAuthors(context);
        }

        private static void SeedBooksAuthors(AppDbContext context)
        {
            if(!context.Authors.Any())
            {
                var authors = GetAuthors();

                context.Authors.AddRange(authors);
                context.SaveChanges();
            }
        }

        private static List<Author> GetAuthors()
        {
            List<Author> authors =
            [
                new Author
                {
                    Name = "George Orwell",
                    Biography = "Британський письменник та журналіст.",
                    Country = "United Kingdom",
                    BirthDate = new DateTime(1903, 6, 25).ToUniversalTime(),
                    Books = GenerateBooks(20)
                },

                new Author
                {
                    Name = "Jane Austen",
                    Biography = "Англійська письменниця, відома романами про кохання та суспільство.",
                    Country = "United Kingdom",
                    BirthDate = new DateTime(1775, 12, 16).ToUniversalTime(),
                    Books = GenerateBooks(20)
                },

                new Author
                {
                    Name = "Ernest Hemingway",
                    Biography = "Американський письменник та журналіст, лауреат Нобелівської премії.",
                    Country = "United States",
                    BirthDate = new DateTime(1899, 7, 21).ToUniversalTime(),
                    Books = GenerateBooks(20)
                },

                new Author
                {
                    Name = "Leo Tolstoy",
                    Biography = "Російський письменник та філософ.",
                    Country = "Russia",
                    BirthDate = new DateTime(1828, 9, 9).ToUniversalTime(),
                    Books = GenerateBooks(20)
                },

                new Author
                {
                    Name = "Fyodor Dostoevsky",
                    Biography = "Російський письменник, філософ та публіцист.",
                    Country = "Russia",
                    BirthDate = new DateTime(1821, 11, 11).ToUniversalTime(),
                    Books = GenerateBooks(20)
                },

                new Author
                {
                    Name = "Mark Twain",
                    Biography = "Американський письменник та гуморист.",
                    Country = "United States",
                    BirthDate = new DateTime(1835, 11, 30).ToUniversalTime(),
                    Books = GenerateBooks(20)
                },

                new Author
                {
                    Name = "William Shakespeare",
                    Biography = "Англійський драматург та поет.",
                    Country = "United Kingdom",
                    BirthDate = new DateTime(1564, 4, 26).ToUniversalTime(),
                    Books = GenerateBooks(20)
                },

                new Author
                {
                    Name = "Charles Dickens",
                    Biography = "Один із найвідоміших англійських письменників XIX століття.",
                    Country = "United Kingdom",
                    BirthDate = new DateTime(1812, 2, 7).ToUniversalTime(),
                    Books = GenerateBooks(20)
                },

                new Author
                {
                    Name = "Victor Hugo",
                    Biography = "Французький письменник, поет та драматург.",
                    Country = "France",
                    BirthDate = new DateTime(1802, 2, 26).ToUniversalTime(),
                    Books = GenerateBooks(20)
                },

                new Author
                {
                    Name = "Jules Verne",
                    Biography = "Французький письменник, один із засновників наукової фантастики.",
                    Country = "France",
                    BirthDate = new DateTime(1828, 2, 8).ToUniversalTime(),
                    Books = GenerateBooks(20)
                },

                new Author
                {
                    Name = "Arthur Conan Doyle",
                    Biography = "Шотландський письменник, творець Шерлока Холмса.",
                    Country = "United Kingdom",
                    BirthDate = new DateTime(1859, 5, 22).ToUniversalTime(),
                    Books = GenerateBooks(20)
                },

                new Author
                {
                    Name = "Agatha Christie",
                    Biography = "Британська письменниця детективів.",
                    Country = "United Kingdom",
                    BirthDate = new DateTime(1890, 9, 15).ToUniversalTime(),
                    Books = GenerateBooks(20)
                },

                new Author
                {
                    Name = "Stephen King",
                    Biography = "Американський письменник, відомий творами жахів та трилерів.",
                    Country = "United States",
                    BirthDate = new DateTime(1947, 9, 21).ToUniversalTime(),
                    Books = GenerateBooks(20)
                },

                new Author
                {
                    Name = "J. K. Rowling",
                    Biography = "Британська письменниця, авторка серії про Гаррі Поттера.",
                    Country = "United Kingdom",
                    BirthDate = new DateTime(1965, 7, 31).ToUniversalTime(),
                    Books = GenerateBooks(20)
                },

                new Author
                {
                    Name = "Gabriel García Márquez",
                    Biography = "Колумбійський письменник та лауреат Нобелівської премії.",
                    Country = "Colombia",
                    BirthDate = new DateTime(1927, 3, 6).ToUniversalTime(),
                    Books = GenerateBooks(20)
                },

                new Author
                {
                    Name = "Haruki Murakami",
                    Biography = "Японський письменник та перекладач.",
                    Country = "Japan",
                    BirthDate = new DateTime(1949, 1, 12).ToUniversalTime(),
                    Books = GenerateBooks(20)
                },

                new Author
                {
                    Name = "Paulo Coelho",
                    Biography = "Бразильський письменник, автор філософських романів.",
                    Country = "Brazil",
                    BirthDate = new DateTime(1947, 8, 24).ToUniversalTime(),
                    Books = GenerateBooks(20)
                },

                new Author
                {
                    Name = "Dan Brown",
                    Biography = "Американський письменник, автор популярних трилерів.",
                    Country = "United States",
                    BirthDate = new DateTime(1964, 6, 22).ToUniversalTime(),
                    Books = GenerateBooks(20)
                },

                new Author
                {
                    Name = "J. R. R. Tolkien",
                    Biography = "Англійський письменник та філолог, автор всесвіту Середзем'я.",
                    Country = "United Kingdom",
                    BirthDate = new DateTime(1892, 1, 3).ToUniversalTime(),
                    Books = GenerateBooks(20)
                },

                new Author
                {
                    Name = "Frank Herbert",
                    Biography = "Американський письменник-фантаст, автор серії Dune.",
                    Country = "United States",
                    BirthDate = new DateTime(1920, 10, 8).ToUniversalTime(),
                    Books = GenerateBooks(20)
                }
            ];

            return authors;
        }

        private static List<Book> GenerateBooks(int count)
        {
            var books = new List<Book>();

            string[] titles =
            [
                "The Beginning",
                "The Last Journey",
                "Darkness",
                "The Lost World",
                "New Horizons",
                "The Secret",
                "Beyond the Stars",
                "The Forgotten Road",
                "Winter Dreams",
                "The Final Chapter",
                "Shadows",
                "The Unknown",
                "A New Beginning",
                "The Hidden Truth",
                "The Great Adventure",
                "Into the Future",
                "The Silent City",
                "The Long Road",
                "The Last Hope",
                "The New World"
            ];

            string[] descriptions =
            [
                "Захоплива історія про людей та їхні долі.",
                "Роман про подорож, відкриття та несподівані пригоди.",
                "Історія, сповнена таємниць та загадок.",
                "Захоплива розповідь про боротьбу та силу духу.",
                "Історія про пошук себе та свого місця у світі."
            ];

            for (int i = 0; i < count; i++)
            {
                books.Add(new Book
                {
                    Title = $"{titles[i % titles.Length]}",
                    Description = descriptions[i % descriptions.Length],
                    Rating = Random.Shared.Next(1, 11),
                    Price = Random.Shared.Next(100, 1000),
                    Pages = Random.Shared.Next(150, 801),
                    Year = Random.Shared.Next(1950, 2026)
                });
            }

            return books;
        }
    }
}
