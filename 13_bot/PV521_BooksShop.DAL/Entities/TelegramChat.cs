namespace PV521_BooksShop.DAL.Entities
{
    public class TelegramChat : BaseEntity
    {
        public long ChatId { get; set; }
        public string? Title { get; set; }
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool IsSubscribe { get; set; } = false;
        public string LastCommand { get; set; } = "None";
    }
}
