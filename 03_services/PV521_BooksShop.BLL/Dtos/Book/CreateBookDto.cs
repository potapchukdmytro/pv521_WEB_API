namespace PV521_BooksShop.BLL.Dtos.Book
{
    public class CreateBookDto
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int Rating { get; set; }
        public decimal Price { get; set; }
        public int Pages { get; set; }
        public int Year { get; set; }
        public int? AuthorId { get; set; }
    }
}
