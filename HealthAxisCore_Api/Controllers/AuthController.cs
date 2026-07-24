using HealthAxisCore_Api.Models.Dtos;
using HealthAxisCore_Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxisCore_Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(
        IAuthService authService
    ) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register(
            RegisterPatientDto request,
            CancellationToken ct)
        {
            var response =
                await authService.RegisterPatientAsync(
                    request,
                    ct);

            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(
            LoginDto request,
            CancellationToken ct)
        {
            var response =
                await authService.LoginAsync(
                    request,
                    ct);

            return Ok(response);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<AuthResponseDto>> RefreshToken(
            RefreshTokenRequestDto request,
            CancellationToken ct)
        {
            var response =
                await authService.RefreshTokenAsync(
                    request,
                    ct);

            return Ok(response);
        }

        [HttpPost("change-first-login-password")]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<string>>
            ChangeFirstLoginPassword(
                ChangeFirstLoginPasswordDto request,
                CancellationToken ct)
        {
            var response =
                await authService.ChangeFirstLoginPasswordAsync(
                    request,
                    User,
                    ct);

            return Ok(response);
        }

        [HttpPost("change-password")]
        [Authorize(Roles = "Patient,Doctor")]
        public async Task<ActionResult<string>> ChangePassword(
            ChangePasswordDto request,
            CancellationToken ct)
        {
            var response =
                await authService.ChangePasswordAsync(
                    request,
                    User,
                    ct);

            return Ok(response);
        }

        [HttpPost("forgot-password")]
        public async Task<ActionResult<ForgotPasswordResponseDto>>
            ForgotPassword(
                ForgotPasswordDto request)
        {
            var response =
                await authService.ForgotPasswordAsync(
                    request);

            return Ok(response);
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult<string>> ResetPassword(
            ResetPasswordDto request)
        {
            var response =
                await authService.ResetPasswordAsync(
                    request);

            return Ok(response);
        }
    }
}