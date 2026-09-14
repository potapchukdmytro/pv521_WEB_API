using Microsoft.AspNetCore.Mvc;
using PV521_BooksShop.BLL.Dtos;

namespace PV521_BooksShop.Extensions
{
    public static class ExstensionMethods
    {
        public static IActionResult GetHttpResponse(this ControllerBase controller, ServiceResponseDto dto)
        {
            return dto.IsSuccess ? controller.Ok(dto) : controller.BadRequest(dto);
        }
    }
}
