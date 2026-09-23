namespace PV521_BooksShop.DAL.Entities
{
    public class Role : BaseEntity
    {
        public required string Name { get; set; }

        public List<User> Users { get; set; } = [];
    }
}
