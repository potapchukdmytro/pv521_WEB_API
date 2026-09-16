using FluentValidation.Results;
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

        public static IActionResult GetValiationErrorResponse(this ControllerBase controller, ValidationResult validation)
        {
            var errors = new Dictionary<string, string>();

            foreach (var error in validation.Errors)
            {
                errors.Add(error.PropertyName, error.ErrorMessage);
            }

            var responseDto = new ServiceResponseDto
            {
                IsSuccess = false,
                Message = "Помилка валідації",
                Payload = errors
            };

            return controller.BadRequest(responseDto);
        }
    }
}
