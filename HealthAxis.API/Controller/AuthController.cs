using HealthAxis.API.DTO.AuthDtos;
using HealthAxis.API.DTO.DoctorDtos;
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
                var (success, message, userId, StatusCode) = await service.Register(request);

                if (!success)
                {
                    return BadRequest(new { message });
                }

                return Ok(new { message, userId });
            }

            [HttpPost("login")]
            public async Task<IActionResult> Login(LoginDto request)
            {
                var (success, message, token, ExpiresIn) = await service.Login(request);

                if (!success)
                {
                    return Unauthorized(new { message });
                }
                AuthResponse response = new AuthResponse
                {
                    AccessToken = token,
                    Message = message,
                    ExpiresIn = ExpiresIn
                };

                return Ok(response);
            }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto request)
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

