using HealthAxis.Shared.DTOs.Auth;
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

            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new
            {
                message = result.Message,
                userId = result.UserId
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto request)
        {
            var result = await _service.Login(request);

            if (!result.Success)
            {
                return Unauthorized(new { message = result.Message });
            }

            return Ok(new AuthResponse
            {
                AccessToken = result.AccessToken,
                RefreshToken = result.RefreshToken,
                Message = result.Message,
                ExpiresIn = result.ExpiresIn,
                RequiresPasswordChange = result.RequiresPasswordChange
            });
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto request)
        {
            var result = await _service.ChangePassword(request);

            if (!result.Success)
            {
                return BadRequest(new { message = result.Message });
            }

            return Ok(new { message = result.Message });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequestDto request)
        {
            var result = await _service.RefreshToken(request);

            if (!result.Success)
            {
                return Unauthorized(new { message = result.Message });
            }

            return Ok(new AuthResponse
            {
                AccessToken = result.AccessToken,
                RefreshToken = result.RefreshToken,
                Message = result.Message,
                ExpiresIn = result.ExpiresIn,
                RequiresPasswordChange = false
            });
        }
    }
}