using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PV521_BooksShop.BLL.Settings;
using PV521_BooksShop.DAL.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PV521_BooksShop.BLL.Services
{
    public class JwtService
    {
        private readonly JwtSettings _settings;

        public JwtService(IOptions<JwtSettings> options)
        {
            _settings = options.Value;

            if(string.IsNullOrEmpty(_settings.SecretKey))
            {
                throw new ArgumentNullException("Secret is null");
            }
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
                expires: DateTime.UtcNow.AddMinutes(_settings.ExpHours),
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
    }
}
