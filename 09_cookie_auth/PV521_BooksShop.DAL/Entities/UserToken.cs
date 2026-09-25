namespace PV521_BooksShop.DAL.Entities
{
    public class UserToken : BaseEntity
    {
        public required string Token { get; set; }
        public DateTime Expires { get; set; } = DateTime.UtcNow.AddMinutes(15);

        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
