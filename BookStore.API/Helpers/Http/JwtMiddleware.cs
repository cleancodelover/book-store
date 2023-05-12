using BookStore.API.Helpers.Http.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BookStore.API.Helpers.Http
{
        public class JwtMiddleware
        {
            private readonly RequestDelegate _next;

            public JwtMiddleware(RequestDelegate next)
            {
                _next = next;
            }

            public async Task InvokeAsync(HttpContext context)
            {
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (token != null)
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                    // Extract claims from the token
                    var claims = jwtToken?.Claims;

                    // Add the claims to the HttpContext User
                    context.User = new ClaimsPrincipal(new ClaimsIdentity(claims));
                }

                await _next(context);
            }
        }
}
