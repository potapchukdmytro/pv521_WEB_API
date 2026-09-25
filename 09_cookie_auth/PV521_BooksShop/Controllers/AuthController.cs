using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PV521_BooksShop.BLL.Dtos;
using PV521_BooksShop.BLL.Dtos.Auth;
using PV521_BooksShop.BLL.Services;
using PV521_BooksShop.BLL.Settings;
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
        private readonly JwtSettings _jwtSettings;

        public AuthController(AuthService authService, IValidator<RegisterDto> registerValidator, IValidator<LoginDto> loginValidator, IOptions<JwtSettings> options)
        {
            _authService = authService;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
            _jwtSettings = options.Value;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto dto, [FromQuery] bool authorization = true, CancellationToken ct = default)
        {
            var validation = await _registerValidator.ValidateAsync(dto, ct);

            if (!validation.IsValid)
            {
                return this.GetValiationErrorResponse(validation);
            }

            var response = await _authService.RegisterAsync(dto, ct);

            if (response.IsSuccess)
            {
                var token = response.Payload?.ToString();
                if (token != null)
                {
                    if (authorization)
                    {
                        // Запис у cookie
                        var options = new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true,
                            SameSite = SameSiteMode.None,
                            Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpHours)
                        };

                        Response.Cookies.Append("accessToken", token, options);
                    }

                    response.Payload = null;
                    return Ok(response);
                }
            }

            return BadRequest(response);
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

            if (response.IsSuccess)
            {
                var token = response.Payload?.ToString();
                if (token != null)
                {
                    // Запис у cookie
                    var options = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.None,
                        Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpHours)
                    };

                    Response.Cookies.Append("accessToken", token, options);

                    response.Payload = null;
                    return Ok(response);
                }
            }

            return BadRequest(response);
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> MeAsync(CancellationToken ct = default)
        {
            var id = User.FindFirst("id")?.Value;

            if (id == null)
            {
                return Unauthorized(ServiceResponseDto.Error("Невалідний токен"));
            }

            int userId = int.Parse(id);

            try
            {
                var response = await _authService.UserDataAsync(userId, ct);
                return this.GetHttpResponse(response);
            }
            catch (Exception)
            {
                return Unauthorized(ServiceResponseDto.Error("Невалідний токен"));
            }
        }

        [HttpPost("sendEmailConfirmMessage")]
        public async Task<IActionResult> SendConfirmEmailAsync([FromBody] SendConfirmEmailDto dto, [FromQuery] string? callbackUrl, CancellationToken ct = default)
        {
            if(string.IsNullOrEmpty(callbackUrl))
            {
                callbackUrl = $"{Request.Scheme}://{Request.Host}/api/auth/confirmEmail";
            }

            var response = await _authService.SendConfirmEmailAsync(dto, callbackUrl, ct);
            return this.GetHttpResponse(response);
        }

        [HttpGet("confirmEmail")]
        public async Task<IActionResult> SendConfirmEmailAsyc([FromQuery] int uid, [FromQuery]  string token, CancellationToken ct = default)
        {
            var response = await _authService.ConfirmEmailAsync(uid, token, ct);
            return this.GetHttpResponse(response);
        }
    }
}
