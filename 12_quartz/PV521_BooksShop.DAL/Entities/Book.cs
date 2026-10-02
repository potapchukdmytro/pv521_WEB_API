namespace PV521_BooksShop.DAL.Entities
{
    public class Book : BaseEntity
    {
        public required string Title { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public int Rating { get; set; }
        public decimal Price { get; set; }
        public int Pages { get; set; }
        public int Year { get; set; }
        public int Reviews { get; set; }
        public string? Language { get; set; }
        public string? Publisher { get; set; }
        public string? Isbn { get; set; }

        public int? AuthorId { get; set; }
        public Author? Author { get; set; }
    }
}
