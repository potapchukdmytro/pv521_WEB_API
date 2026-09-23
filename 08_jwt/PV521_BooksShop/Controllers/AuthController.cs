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
        private readonly IValidator<RegisterDto> _registerValidator;
        private readonly IValidator<LoginDto> _loginValidator;
        private readonly JwtService _jwtService;

        public AuthController(AuthService authService, IValidator<RegisterDto> registerValidator, IValidator<LoginDto> loginValidator, JwtService jwtService)
        {
            _authService = authService;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
            _jwtService = jwtService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto dto, CancellationToken ct = default)
        {
            var validation = await _registerValidator.ValidateAsync(dto, ct);

            if(!validation.IsValid)
            {
                return this.GetValiationErrorResponse(validation);
            }

            var response = await _authService.RegisterAsync(dto, ct);
            return this.GetHttpResponse(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto dto, CancellationToken ct = default)
        {
            var validation = await _loginValidator.ValidateAsync(dto, ct);

            if (!validation.IsValid)
            {
                return this.GetValiationErrorResponse(validation);
            }

            var response = await _authService.LoginAsync(dto, ct);
            return this.GetHttpResponse(response);
        }

        [HttpPost("validate")]
        public async Task<IActionResult> ValidateToken([FromBody] string token)
        {
            bool res = _jwtService.ValidateAccessToken(token);

            if(res)
            {
                return Ok(res);
            }
            else
            {
                return Unauthorized(res);
            }
        }
    }
}
