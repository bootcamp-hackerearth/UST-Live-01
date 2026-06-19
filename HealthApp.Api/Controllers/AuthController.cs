using HealthApp.Api.Dto;
using HealthApp.Api.Model;
using HealthApp.Api.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace HealthApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto register)
        {
            var (success, message, userId) = await authService.Register(register);
            if(!success) {
                return BadRequest(message);
            }
            return Ok(new { message, userId });

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            var (success, message, accessToken,expiresIn) = await authService.Login(login);
            if(!success) {
                return BadRequest(message);
            }
            return Ok(new AuthResponse
            {
                AccessToken = accessToken,
                ExpiresIn = expiresIn,
                message = message
            });
        }




    }
}
