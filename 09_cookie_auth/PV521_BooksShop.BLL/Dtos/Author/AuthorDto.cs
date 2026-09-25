namespace PV521_BooksShop.BLL.Dtos.Author
{
    public class AuthorDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Biography { get; set; }
        public string? Image { get; set; }
        public string? Country { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
