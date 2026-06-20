using HealthApp.Api.Dtos;
using HealthApp.Api.Exceptions;
using HealthApp.Api.Extensions;
using HealthApp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService service) : ControllerBase
    {
        [HttpPost("register/patient")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterPatient(RegisterPatientDto request)
        {
            var (success, message, userId) = await service.RegisterPatient(request);

            if (!success)
            {
                return BadRequest(new { message });
            }

            return Ok(new
            {
                message,
                userId
            });
        }

        [HttpPost("register/doctor")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> RegisterDoctor(DoctorCreateDto request)
        {
            var (success, message, userId, temporaryPassword) = await service.RegisterDoctor(request);

            if (!success)
            {
                return BadRequest(new { message });
            }

            return Ok(new
            {
                message,
                userId,
                temporaryPassword
            });
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto request)
        {
            var (success, message, token, expiresIn) = await service.Login(request);

            if (!success)
            {
                return Unauthorized(new { message });
            }

            var response = new AuthResponse
            {
                AccessToken = token,
                Message = message,
                ExpiresIn = expiresIn
            };

            return Ok(response);
        }

        [HttpPost("change-password")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto request)
        {
            var userId = User.GetUserId();

            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new UnauthorizedAccessAppException("Please login to continue.");
            }

            await service.ChangePasswordAsync(userId, request);

            return Ok(new
            {
                message = "Password changed successfully."
            });
        }

    }
}