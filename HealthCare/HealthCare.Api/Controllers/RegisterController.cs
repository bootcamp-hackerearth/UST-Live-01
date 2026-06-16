using Microsoft.AspNetCore.Mvc;
using HealthCare.Api.DTOs.Authentication;
using HealthCare.Api.Services.Interfaces;
using HealthCare.Api.Models;


namespace HealthCare.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IAuthService _service;

        public RegisterController(IAuthService service)
        {
            _service = service;
        }

        // Register
        [HttpPost("register")]
        public async Task<IActionResult> Register( RegisterDto request)
        {
            
            var (success, message, userId) = await _service.Register(request);

            if (!success)
            {
                return BadRequest(new { success = false, message });
            }

            return Ok(new { success = true, message, userId });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            var (success, message, token, expiresIn) = await _service.Login(request);

            if (!success)
            {
                return Unauthorized(new { success = false, message });
            }

            var response = new AuthorResponse
            {
                AccessToken = token,
                Message = message,
                ExpiresIn = expiresIn
            };

            return Ok(response);
        }
    }
}