using HealthAxis.API.Models.Auth;
using HealthAxis.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxis.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto request)
        {
            var result = await _service.Register(request);

            if (!result.Item1)
                return BadRequest(result.Item2);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto request)
        {
            var result = await _service.Login(request);

            if (!result.Item1)
                return Unauthorized(result.Item2);

            return Ok(new AuthResponse
            {
                AccessToken = result.Item3,
                Message = result.Item2,
                ExpiresIn = result.Item4
            });
        }
    }
}
