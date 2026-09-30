using Microsoft.AspNetCore.Http;

namespace PV521_BooksShop.BLL.Dtos.User
{
    public class SetAvatarDto
    {
        public int UserId { get; set; }
        public required IFormFile Image { get; set; }
    }
}
