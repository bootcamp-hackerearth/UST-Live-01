using HealthAxisApplicn.Dto.Auth;
using HealthAxisApplicn.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxisApplicn.Controllers
{
    [Route("api/auth")]
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
            var (success, message, token, expiry, refreshToken) = await authService.LoginAsync(request);
            if (!success)
            {
                return Unauthorized(new { message });
            }


            AuthResponse response = new AuthResponse
            {
                AccessToken = token,
                RefreshToken = refreshToken,
                Message = message,
                ExpiresIn = expiry
            };


            return Ok(response);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] string refreshToken)
        {
            var response = await authService.RefreshAsync(refreshToken);

            if (response == null)
            {
                return Unauthorized(new { message = "Invalid or expired refresh token" });
            }

            return Ok(response);
        }
    }
}
