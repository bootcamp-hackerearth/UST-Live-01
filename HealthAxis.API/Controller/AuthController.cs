using HealthAxis.Shared.DTO.AuthDtos;
using HealthAxis.Shared.DTO.DoctorDtos;
using HealthAxis.API.Models;
using HealthAxis.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthAxis.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService service) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto request)
        {
            var (success, message, userId, statusCode) =
                await service.Register(request);

            if (!success)
            {
                return StatusCode(statusCode, new
                {
                    message
                });
            }

            return Ok(new
            {
                message,
                userId
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (success, message, accessToken, refreshToken, expiresIn) =
                await service.Login(request);

            if (!success)
            {
                return Unauthorized(new
                {
                    message
                });
            }

            var response = new AuthResponse
            {
                Message = message,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresIn = expiresIn
            };

            return Ok(response);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(
            [FromBody] RefreshTokenDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await service.RefreshToken(request);

            if (!result.Success)
            {
                return StatusCode(result.StatusCode, new
                {
                    message = result.Message
                });
            }

            var response = new AuthResponse
            {
                Message = result.Message,
                AccessToken = result.AccessToken,
                RefreshToken = result.RefreshToken,
                ExpiresIn = result.ExpiresIn
            };

            return Ok(response);
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(
            [FromBody] ChangePasswordDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized(new
                {
                    message = "Invalid token"
                });
            }

            var result = await service.ChangePassword(userId, request);

            if (!result.Success)
            {
                return StatusCode(result.StatusCode, new
                {
                    message = result.Message
                });
            }

            return Ok(new
            {
                message = result.Message
            });
        }
    }
}