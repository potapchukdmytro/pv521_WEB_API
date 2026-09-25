using AutoMapper;
using PV521_BooksShop.BLL.Dtos;
using PV521_BooksShop.BLL.Dtos.Auth;
using PV521_BooksShop.BLL.Dtos.User;
using PV521_BooksShop.DAL.Entities;
using PV521_BooksShop.DAL.Repositories;

namespace PV521_BooksShop.BLL.Services
{
    public class AuthService
    {
        private readonly UserService _userService;
        private readonly UserRepository _userRepository;
        private readonly JwtService _jwtService;
        private readonly IMapper _mapper;

        public AuthService(IMapper mapper, UserService userService, JwtService jwtService, UserRepository userRepository)
        {
            _mapper = mapper;
            _userService = userService;
            _jwtService = jwtService;
            _userRepository = userRepository;
        }

        public async Task<ServiceResponseDto> LoginAsync(LoginDto dto, CancellationToken ct = default)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email, ct);

            if(user == null)
            {
                return ServiceResponseDto.Error("Невірна пошта");
            }

            var passResult = _userService.CheckPassword(user, dto.Password);

            if(!passResult)
            {
                return ServiceResponseDto.Error("Невірний пароль");
            }

            var token = _jwtService.GenerateAccessToken(user);

            return ServiceResponseDto.Success("Успішний вхід", token);
        }

        public async Task<ServiceResponseDto> UserDataAsync(int userId, CancellationToken ct = default)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId, ct);

                if(user == null)
                {
                    return ServiceResponseDto.Error($"Користувач з id '{userId}' не знайдений");
                }

                return ServiceResponseDto.Success("Дані користувача отримано", _mapper.Map<UserDto>(user));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ServiceResponseDto> RegisterAsync(RegisterDto dto, CancellationToken ct = default)
        {
            var user = _mapper.Map<User>(dto);

            await _userService.CreateAsync(user, dto.Password, ct);

            // Jwt Token
            string token = _jwtService.GenerateAccessToken(user);

            return ServiceResponseDto.Success("Користувач успішно зареєстрований", token);
        }
    }
}
