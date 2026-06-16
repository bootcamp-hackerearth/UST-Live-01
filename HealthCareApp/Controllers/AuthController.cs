using HealthCareApp.Models;
using HealthCareApp.Models.Dtos;
using HealthCareApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace HealthCareApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService service) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto request)
        {
            var (success, message, userId) = await service.Register(request);

            if (!success)
            {
                return BadRequest(new
                {
                    Message = message
                });
            }

            return Ok(new
            {
                Message = message,
                UserId = userId
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto request)
        {
            var (success, message, token, expiresIn) = await service.Login(request);

            if (!success)
            {
                return Unauthorized(new
                {
                    Message = message
                });
            }

            AuthResponse response = new AuthResponse
            {
                AccessToken = token,
                Message = message,
                ExpiresIn = expiresIn
            };

            return Ok(response);
        }
    }
}