using System.ComponentModel.DataAnnotations;

namespace PV521_BooksShop.Dtos
{
    public class AuthorDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string? Biography { get; set; }
        public string? Image { get; set; }
        public string? Country { get; set; }
        public DateTime BirthDate { get; set; }
    }

    public class CreateAuthorDto
    {
        [Required]
        public string Name { get; set; } = "";

        public string? Biography { get; set; }
        public string? Image { get; set; }
        public string? Country { get; set; }

        [Required]
        public DateTime? BirthDate { get; set; }
    }

    public class UpdateAuthorDto : CreateAuthorDto
    {
    }

    public class AuthorBookDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string? Description { get; set; }
        public string? Image { get; set; }
        public int Rating { get; set; }
        public decimal Price { get; set; }
        public int Pages { get; set; }
        public int Year { get; set; }
        public int? AuthorId { get; set; }
    }
}
