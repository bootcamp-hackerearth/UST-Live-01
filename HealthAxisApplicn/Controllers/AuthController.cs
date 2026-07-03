using HealthAxisApplicn.Dto.Auth;
using HealthAxisApplicn.Models;
using HealthAxisApplicn.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxisApplicn.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;
        private readonly UserManager<ApplicationUser> userManager;

        public AuthController(IAuthService authService,
                              UserManager<ApplicationUser> userManager) 
        {
            this.authService = authService;
            this.userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto request)
        {
            var result = await authService.RegisterAsync(request);

            if (!result.Success)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto request)
        {
            var result = await authService.LoginAsync(request);

            if (string.IsNullOrEmpty(result.AccessToken))
            {
                return Unauthorized(new { message = result.Message });
            }

            return Ok(result);
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

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var user = await userManager.GetUserAsync(User);

            if (user == null)
                return Unauthorized();

            user.PasswordHash = userManager.PasswordHasher.HashPassword(user, dto.NewPassword);
            user.IsFirstLogin = false;

            await userManager.UpdateAsync(user);

            return Ok(new { message = "Password updated ✅" });
        }
    }
}