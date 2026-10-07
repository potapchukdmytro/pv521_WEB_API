namespace PV521_BooksShop.BLL.Dtos.Parser
{
    public class DisconnectDto
    {
        public string Title { get; set; } = string.Empty;
        public string Today { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"{Title}: {Today}";
        }
    }
}
