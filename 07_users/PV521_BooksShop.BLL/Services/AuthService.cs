using AutoMapper;
using PV521_BooksShop.BLL.Dtos;
using PV521_BooksShop.BLL.Dtos.Auth;
using PV521_BooksShop.DAL.Entities;

namespace PV521_BooksShop.BLL.Services
{
    public class AuthService
    {
        private readonly UserService _userService;
        private readonly IMapper _mapper;

        public AuthService(IMapper mapper, UserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<ServiceResponseDto> RegisterAsync(RegisterDto dto, CancellationToken ct = default)
        {
            var user = _mapper.Map<User>(dto);

            await _userService.CreateAsync(user, dto.Password, ct);

            return ServiceResponseDto.Success("Користувач успішно зареєстрований");
        }
    }
}
