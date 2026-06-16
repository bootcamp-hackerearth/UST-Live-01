using HealthAxisCore_Api.Models;
using HealthAxisCore_Api.Models.DTOs;
using HealthAxisCore_Api.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxisCore_Api.Controllers
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

        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var (success, message) = await service.DeleteUser(id);
            if (!success)
                return NotFound(new { message });

            return Ok(new { message });
        }

    }
}
