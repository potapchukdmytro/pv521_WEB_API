using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PV521_BooksShop.BLL.Dtos;
using PV521_BooksShop.BLL.Dtos.User;
using PV521_BooksShop.DAL.Abstraction;
using PV521_BooksShop.DAL.Entities;
using PV521_BooksShop.DAL.Repositories;

namespace PV521_BooksShop.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly UserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly PasswordHasher<User> _passwordHasher;

        public UserService(UserRepository userRepository, IMapper mapper, PasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }

        public async Task<ServiceResponseDto> GetAllAsync(CancellationToken ct = default)
        {
            var entities = await _userRepository.Users
                .Include(u => u.Role)
                .ToListAsync(ct);

            var dtos = _mapper.Map<List<UserDto>>(entities);

            return ServiceResponseDto.Success("Користувачі успішно отримані", dtos);
        }

        public async Task<ServiceResponseDto> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _userRepository.GetByIdAsync(id, ct);

            if(entity == null)
            {
                return ServiceResponseDto.Error($"Користувач з id '{id}' не знайдений");
            }

            var dto = _mapper.Map<UserDto>(entity);

            return ServiceResponseDto.Success("Користувач успішно отриманий", dto);
        }

        public async Task CreateAsync(User user, string password, CancellationToken ct = default)
        {
            var passwordHash = _passwordHasher.HashPassword(user, password);
            user.PasswordHash = passwordHash;

            await _userRepository.CreateAsync(user, ct);
        }

        public bool CheckPassword(User user, string password)
        {
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

            return result == PasswordVerificationResult.Success
                || result == PasswordVerificationResult.SuccessRehashNeeded;
        }
    }
}
