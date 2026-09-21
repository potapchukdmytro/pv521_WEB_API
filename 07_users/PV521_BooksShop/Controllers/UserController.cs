using Microsoft.AspNetCore.Mvc;
using PV521_BooksShop.BLL.Dtos;
using PV521_BooksShop.BLL.Dtos.User;
using PV521_BooksShop.BLL.Services;
using PV521_BooksShop.Extensions;

namespace PV521_BooksShop.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
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

            return Ok();
        }
    }
}
