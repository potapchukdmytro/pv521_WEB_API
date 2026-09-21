using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using PV521_BooksShop.BLL.Dtos.Auth;
using PV521_BooksShop.BLL.Services;
using PV521_BooksShop.Extensions;

namespace PV521_BooksShop.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly IValidator<RegisterDto> _validator;

        public AuthController(AuthService authService, IValidator<RegisterDto> validator)
        {
            _authService = authService;
            _validator = validator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterDto dto, CancellationToken ct = default)
        {
            var validation = await _validator.ValidateAsync(dto, ct);

            if(!validation.IsValid)
            {
                return this.GetValiationErrorResponse(validation);
            }

            var response = await _authService.RegisterAsync(dto, ct);
            return this.GetHttpResponse(response);
        }
    }
}
