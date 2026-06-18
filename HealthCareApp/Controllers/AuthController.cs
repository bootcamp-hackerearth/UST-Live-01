using HealthCareApp.Dtos;
using HealthCareApp.Models;
using HealthCareApp.Models.Dtos;
using HealthCareApp.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthCareApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService service) : ControllerBase
    {
        [HttpPost("register-patient")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterPatient(PatientRegisterDto request)
        {
            var (success, message, userId) = await service.RegisterPatientAsync(request);

            if (!success)
            {
                return BadRequest(new
                {
                    Message = message
                });
            }

            return Ok(new
            {
                Message = message,
                UserId = userId
            });
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto request)
        {
            var (success, message, token, expiresIn) = await service.Login(request);

            if (!success)
            {
                return Unauthorized(new
                {
                    Message = message
                });
            }

            AuthResponse response = new AuthResponse
            {
                AccessToken = token,
                Message = message,
                ExpiresIn = expiresIn
            };

            return Ok(response);
        }

        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto request)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized(new
                {
                    Message = "Invalid user token."
                });
            }

            var (success, message) = await service.ChangePasswordAsync(userId, request);

            if (!success)
            {
                return BadRequest(new
                {
                    Message = message
                });
            }

            return Ok(new
            {
                Message = message
            });
        }
    }
}