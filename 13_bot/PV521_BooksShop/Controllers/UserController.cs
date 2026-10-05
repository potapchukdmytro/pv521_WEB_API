using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PV521_BooksShop.BLL.Dtos;
using PV521_BooksShop.BLL.Dtos.User;
using PV521_BooksShop.BLL.Services;
using PV521_BooksShop.Extensions;
using PV521_BooksShop.Settings;

namespace PV521_BooksShop.Controllers
{
    [ApiController]
    [Route("api/user")]
    [Authorize(Roles = "admin")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly string _imagesPath;
        private readonly ILogger<UserController> _logger;

        public UserController(UserService userService, IWebHostEnvironment env, ILogger<UserController> logger)
        {
            _userService = userService;
            _imagesPath = Path.Combine(env.ContentRootPath, PathSettings.Users);
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(CancellationToken ct = default)
        {
            var response = await _userService.GetAllAsync(ct);
            return this.GetHttpResponse(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id, CancellationToken ct = default)
        {
            var response = await _userService.GetByIdAsync(id, ct);

            if (response.IsSuccess)
            {
                _logger.LogInformation(1002, $"Get user by id {id} success");
            }
            else
            {
                //_logger.LogInformation(9035, $"Get user by id {id} failed - {response.Message}");
                //_logger.LogWarning(9035, $"Get user by id {id} failed - {response.Message}");
                _logger.LogError(9035, $"Get user by id {id} failed - {response.Message}");
                //_logger.LogCritical(9035, $"Get user by id {id} failed - {response.Message}");
            }

            return this.GetHttpResponse(response);
        }

        [HttpPatch("avatar")]
        public async Task<IActionResult> SetAvatarAsync([FromForm] SetAvatarDto dto, CancellationToken ct = default)
        {
            if(dto.Image == null)
            {
                return BadRequest(ServiceResponseDto.Error("Зображення є обов'язковим"));
            }

            var response = await _userService.SetAvatarAsync(dto, _imagesPath, ct);

            return this.GetHttpResponse(response);
        }

        [HttpPatch("role")]
        public async Task<IActionResult> SetAvatarAsync([FromForm] SetRoleDto dto, CancellationToken ct = default)
        {
            var response = await _userService.AddToRoleAsync(dto, ct);
            return this.GetHttpResponse(response);
        }
    }
}
