using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PV521_BooksShop.BLL.Dtos;
using PV521_BooksShop.BLL.Settings;
using PV521_BooksShop.DAL;
using PV521_BooksShop.DAL.Entities;
using PV521_BooksShop.DAL.Migrations;
using PV521_BooksShop.DAL.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PV521_BooksShop.BLL.Services
{
    public class JwtService
    {
        private readonly UserTokenRepository _userTokenRepository;
        private readonly UserRepository _userRepository;
        private readonly JwtSettings _settings;

        public JwtService(IOptions<JwtSettings> options, UserTokenRepository userTokenRepository, UserRepository userRepository)
        {
            _settings = options.Value;

            if (string.IsNullOrEmpty(_settings.SecretKey))
            {
                throw new ArgumentNullException("Secret is null");
            }

            _userTokenRepository = userTokenRepository;
            _userRepository = userRepository;
        }

        public string GenerateAccessToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim("id", user.Id.ToString()),
                new Claim("email", user.Email),
                new Claim("userName", user.UserName)
            };

            var bytes = Encoding.UTF8.GetBytes(_settings.SecretKey);
            var key = new SymmetricSecurityKey(bytes);
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                expires: DateTime.UtcNow.AddHours(_settings.ExpHours),
                claims: claims,
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool ValidateAccessToken(string token)
        {
            var keyBytes = Encoding.UTF8.GetBytes(_settings.SecretKey);
            var symmetricKey = new SymmetricSecurityKey(keyBytes);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = _settings.Issuer,
                ValidAudience = _settings.Audience,
                IssuerSigningKey = symmetricKey,
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                tokenHandler.ValidateToken(token, validationParameters, out var securityToken);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int GetUserId(string token)
        {
            bool isValid = ValidateAccessToken(token);

            if(!isValid)
            {
                throw new SecurityTokenArgumentException("Invalid token");
            }

            var handler = new JwtSecurityTokenHandler();

            if(!handler.CanReadToken(token))
            {
                throw new SecurityTokenArgumentException("Invalid token");
            }

            var jwt = handler.ReadJwtToken(token);
            var idValue = (jwt.Claims.FirstOrDefault(c => c.Type == "id")?.Value) 
                ?? throw new SecurityTokenArgumentException("Claim id not found");

            bool parseRes = int.TryParse(idValue, out int userId);

            return parseRes ? userId : throw new FormatException("User id incorrect");
        }

        // Email confirmation
        public string GenerateSecureToken()
        {
            var randomBytes = new byte[32];
            using var rnd = RandomNumberGenerator.Create();
            rnd.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes)
                .Replace("+", "-")
                .Replace("/", "_")
                .Replace("=", "");
        }

        public async Task<string> GenerateEmailConfirmTokenAsync(User user, CancellationToken ct = default)
        {
            var secureToken = GenerateSecureToken();
            var token = new UserToken
            {
                Token = secureToken,
                User = user
            };

            await _userTokenRepository.CreateAsync(token, ct);
            return token.Token;
        }

        public async Task<ServiceResponseDto> ConfirmEmailAsync(int userId, string token, CancellationToken ct = default)
        {
            var user = await _userRepository.Users
                .Include(u => u.Tokens)
                .FirstOrDefaultAsync(u => u.Id == userId, ct);

            if(user == null)
            {
                return ServiceResponseDto.Error($"Користувач з id '{userId}' не знайдений");
            }

            var userToken = user.Tokens.FirstOrDefault(t => t.Token == token);

            if(userToken == null)
            {
                return ServiceResponseDto.Error("Невалідний токен");
            }
            
            if(userToken.Expires < DateTime.UtcNow)
            {
                return ServiceResponseDto.Error("Невалідний токен");
            }

            user.EmailConfirmed = true;

            await _userTokenRepository.DeleteAsync(userToken, ct);

            await _userRepository.UpdateAsync(user);

            return ServiceResponseDto.Success("Пошту підтверджено");
        }
    }
}
