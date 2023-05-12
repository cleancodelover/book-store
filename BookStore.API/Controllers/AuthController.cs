using BookStore.API.Helpers;
using BookStore.API.Helpers.Http.Interfaces;
using BookStore.API.ViewModels.Auth;
using BookStore.API.ViewModels.Get;
using BookStore.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookStore.API.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AspNetUser> _userManager;
        private readonly SignInManager<AspNetUser> _signInManager;
        private readonly IAuthentication _authentication;
        public AuthController(UserManager<AspNetUser> userManager, SignInManager<AspNetUser> signInManager, IAuthentication authentication)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authentication = authentication;
        }


        // POST api/<AuthController>
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Post([FromBody] LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var result = await _signInManager.PasswordSignInAsync(user,model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    var token = _authentication.GenerateJwtToken(user);

                    return Ok(new ApiResponse<object>(false, "Success", new { access_token = token }));
                }
                // Generate JWT token
            }

            return Ok(new ApiResponse<object>(false, "Success", "Invalid login attempt"));
        }

        // PUT api/<AuthController>/5
        [HttpGet]
        [Route("logout")]
        public async Task<IActionResult> Get()
        {
            await _signInManager.SignOutAsync();
            return Ok(new ApiResponse<bool>(false, "Success", true));
        }
    }
}
