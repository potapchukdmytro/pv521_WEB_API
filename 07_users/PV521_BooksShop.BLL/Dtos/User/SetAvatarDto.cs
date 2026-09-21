using Microsoft.AspNetCore.Http;

namespace PV521_BooksShop.BLL.Dtos.User
{
    public class SetAvatarDto
    {
        public int Id { get; set; }
        public required IFormFile Image { get; set; }
    }
}
