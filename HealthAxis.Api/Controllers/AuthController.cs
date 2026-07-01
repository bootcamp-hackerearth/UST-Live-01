using HealthAxisCore_Api.Models.Dtos;
using HealthAxisCore_Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxisCore_Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register(
            RegisterPatientDto request,
            CancellationToken ct)
        {
            return Ok(await authService.RegisterPatientAsync(request, ct));
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(
            LoginDto request,
            CancellationToken ct)
        {
            return Ok(await authService.LoginAsync(request, ct));
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<AuthResponseDto>> RefreshToken(
            RefreshTokenRequestDto request,
            CancellationToken ct)
        {
            return Ok(await authService.RefreshTokenAsync(request, ct));
        }

        [HttpPost("change-first-login-password")]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<string>> ChangeFirstLoginPassword(
            ChangeFirstLoginPasswordDto request,
            CancellationToken ct)
        {
            return Ok(await authService.ChangeFirstLoginPasswordAsync(
                request,
                User,
                ct));
        }

        [HttpPost("change-password")]
        [Authorize(Roles = "Patient,Doctor")]
        public async Task<ActionResult<string>> ChangePassword(
    ChangePasswordDto request,
    CancellationToken ct)
        {
            return Ok(await authService.ChangePasswordAsync(
                request,
                User,
                ct));
        }

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