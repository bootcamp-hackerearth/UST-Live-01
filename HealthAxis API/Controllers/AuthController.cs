using HealthAxis.API.DTOs.Auth;
using HealthAxis.API.Services;
using HealthAxis.Shared.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxis.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(
            RegisterPatientDto request,
            CancellationToken ct)
        {
            var (success, message, userId, patientId) =
                await _service.RegisterPatientAsync(request, ct);

            if (!success)
            {
                return BadRequest(new
                {
                    message
                });
            }

            return Ok(new
            {
                message,
                userId,
                patientId
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            LoginDto request,
            CancellationToken ct)
        {
            var (success, message, accessToken, refreshToken, expiresIn) =
                await _service.LoginAsync(request, ct);

            if (!success)
            {
                return Unauthorized(new
                {
                    message
                });
            }

            AuthResponseDto response = new()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Message = message,
                ExpiresIn = expiresIn
            };

            return Ok(response);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(
            RefreshTokenDto request,
            CancellationToken ct)
        {
            var (success, message, accessToken, refreshToken, expiresIn) =
                await _service.RefreshTokenAsync(request, ct);

            if (!success)
            {
                return Unauthorized(new
                {
                    message
                });
            }

            AuthResponseDto response = new()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Message = message,
                ExpiresIn = expiresIn
            };

            return Ok(response);
        }
    }
}
