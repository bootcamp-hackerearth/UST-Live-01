using HealthApp.API.Models;
using HealthApp.API.Models.DTOs;
using HealthApp.API.Repository.Interface;
using HealthApp.API.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthApp.API.Controllers
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
                return BadRequest(new { message });
            }

            return Ok(new { message, userId });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto request)
        {
            var (success, message, token, ExpiresIn) = await service.Login(request);

            if (!success)
            {
                return Unauthorized(new { message });
            }
            AuthResponse response = new AuthResponse
            {
                AccessToken = token,
                Message = message,
                ExpiresIn = ExpiresIn
            };

            return Ok(response);
        }
    }
}
