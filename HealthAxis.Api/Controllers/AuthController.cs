using HealthAxisCore_Api.Models.Dtos;
using HealthAxisCore_Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxisCore_Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("register")] public async Task<ActionResult<AuthResponseDto>> Register(RegisterPatientDto request, CancellationToken ct) => Ok(await authService.RegisterPatientAsync(request, ct));

        [HttpPost("login")] public async Task<ActionResult<AuthResponseDto>> Login(LoginDto request, CancellationToken ct) => Ok(await authService.LoginAsync(request, ct));

        [HttpPost("refresh-token")]
        public async Task<ActionResult<AuthResponseDto>> RefreshToken(RefreshTokenRequestDto request, CancellationToken ct) => Ok(await authService.RefreshTokenAsync(request, ct));

        [HttpPost("forgot-password")]
        public async Task<ActionResult<ForgotPasswordResponseDto>> ForgotPassword(
    ForgotPasswordDto request)
        {
            return Ok(await authService.ForgotPasswordAsync(request));
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult<string>> ResetPassword(
            ResetPasswordDto request)
        {
            return Ok(await authService.ResetPasswordAsync(request));
        }
    }
}