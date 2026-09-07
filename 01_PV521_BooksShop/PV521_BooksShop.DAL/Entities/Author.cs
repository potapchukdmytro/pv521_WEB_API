namespace PV521_BooksShop.DAL.Entities
{
    public class Author
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Biography { get; set; }
        public string? Image { get; set; }
        public string? Country { get; set; }
        public DateTime BirthDate { get; set; } = DateTime.UtcNow;

        public List<Book> Books { get; set; } = [];
    }
}
