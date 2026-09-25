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
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly string _imagesPath;

        public UserController(UserService userService, IWebHostEnvironment env)
        {
            _userService = userService;
            _imagesPath = Path.Combine(env.ContentRootPath, PathSettings.Users);
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
            return this.GetHttpResponse(response);
        }

        [HttpPatch]
        public async Task<IActionResult> SetAvatarAsync([FromForm] SetAvatarDto dto, CancellationToken ct = default)
        {
            if(dto.Image == null)
            {
                return BadRequest(ServiceResponseDto.Error("Зображення є обов'язковим"));
            }

            var response = await _userService.SetAvatarAsync(dto, _imagesPath, ct);

            return this.GetHttpResponse(response);
        }
    }
}
