using Journal_be.Entities;
using Journal_be.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Journal_be.Security
{
    public class TokenJwt
    {
        private readonly IConfiguration _configuration;
        public TokenJwt(IConfiguration config)
        {
            _configuration = config;
        }

        public string GenerateJwtToken(UserEntity user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("Id", user.Id.ToString()),
                new Claim("UserName", user.UserName.ToString()),
                new Claim("Email", user.Email.ToString()),
                new Claim("Phone", user.Phone.ToString()),
                new Claim("CreatedTime", user.CreatedTime.ToString()),
                new Claim("Address", user.Address.ToString()),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("FirstName", user.FirstName.ToString()),
                new Claim("LastName", user.LastName.ToString()),
                new Claim("Image", user.Image.ToString())

            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(100),
                signingCredentials: signIn);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
