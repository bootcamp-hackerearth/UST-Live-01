using HealthAxisApplicn.Dto;
using HealthAxisApplicn.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxisApplicn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto request)
        {
            var (success, message, userId) = await authService.RegisterAsync(request);

            if(!success)
            {
                return BadRequest(new { message });
            }

            return Ok(new { message, userId });

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto request)
        {
            var (success, message, token, expiry) = await authService.LoginAsync(request);

            if(!success)
            {
                return Unauthorized(new { message });
            }

            AuthResponse response = new AuthResponse
            {
                AccessToken = token,
                Message = message,
                ExpiresIn = expiry
            };

            return Ok(response);
        }
    }
}
