using BookStore.DAL.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookStore.API.Helpers.Http.Interfaces
{
    public interface IAuthentication
    {
        public string GenerateJwtToken(AspNetUser user);
    }
}
