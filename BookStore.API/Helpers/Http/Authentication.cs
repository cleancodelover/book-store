using BookStore.API.Helpers.Http.Interfaces;
using BookStore.DAL.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookStore.API.Helpers.Http
{
    public class Authentication: IAuthentication
    {
        private readonly IConfiguration _configuration;
        public Authentication(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerateJwtToken(AspNetUser user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName)
                // Add additional claims as needed
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfiguration:AuthSecret"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["JwtConfiguration:DurationInMinutes"]));

            var token = new JwtSecurityToken(
                _configuration["JwtConfiguration:Issuer"],
                _configuration["JwtConfiguration:Audience"],
                claims,
                expires: expires,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
