namespace PV521_BooksShop.DAL.Entities
{
    public class User : BaseEntity
    {
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public bool EmailConfirmed { get; set; } = false;
        public string PasswordHash { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Image { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? PhoneNumber { get; set; }

        public int? RoleId { get; set; }
        public Role? Role { get; set; }
    }
}
