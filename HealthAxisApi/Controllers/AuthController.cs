using HealthAxis.Shared.DTOs.Auth;
using HealthAxisCore_Api.DTOs.User;
using HealthAxisCore_Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxisCore_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // ✅ REGISTER
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(request);

            return Ok(result);
        }

        // ✅ LOGIN (Updated → returns Access + Refresh Token)
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(request);

            return Ok(result); // should return AuthResponseDto
        }

        // ✅ REFRESH TOKEN (NEW)
        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.RefreshToken))
                return BadRequest("Refresh token is required");

            var result = await _authService.RefreshTokenAsync(request.RefreshToken);

            return Ok(result);
        }

        // ✅ LOGOUT (REVOKE TOKEN) (NEW)
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout([FromBody] RefreshTokenRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.RefreshToken))
                return BadRequest("Refresh token is required");

            await _authService.RevokeRefreshTokenAsync(request.RefreshToken);

            return Ok(new
            {
                Message = "Logged out successfully"
            });
        }

        // ✅ CHANGE PASSWORD
        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _authService.ChangePasswordAsync(request);

            return Ok(new
            {
                Message = "Password changed successfully"
            });
        }
    }
}