using PV521_BooksShop.BLL.Dtos.Author;

namespace PV521_BooksShop.BLL.Dtos.Book
{
    public class BookDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public int Rating { get; set; }
        public decimal Price { get; set; }
        public int Pages { get; set; }
        public int Year { get; set; }
        public AuthorDto? Author { get; set; }
    }
}
