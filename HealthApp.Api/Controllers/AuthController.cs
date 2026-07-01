using HealthApp.Api.Service.Interface;
using HealthApp.Shared.Dto;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost("patientregister")]
        [AllowAnonymous]
        public async Task<IActionResult> PatientRegister([FromBody] PatientRegisterDto request)
        {
            var result = await authService.RegisterPatientAsync(request);
            var (success, message, userId) = result;

            if (!success)
            {
                return BadRequest(message);
            }

            return Ok(new { message, userId });
        }

        [HttpPost("doctorregister")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> DoctorRegister([FromBody] DoctorRegisterDto request)
        {
            var result = await authService.RegisterDoctorByAdminAsync(request);
            var (success, message, userId, temporaryPassword) = result;

            if (!success)
            {
                return BadRequest(message);
            }

            return Ok(new
            {
                message,
                userId,
                temporaryPassword
            });
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterDto register)
        {
            var result = await authService.Register(register);
            var (success, message, userId) = result;

            if (!success)
            {
                return BadRequest(message);
            }

            return Ok(new { message, userId });
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            try
            {
                var (success, message, accessToken, expiresIn, role) =
                    await authService.Login(login);

                return Ok(new LoginResponseDto
                {
                    Success = success,
                    Message = message,
                    AccessToken = accessToken,
                    ExpiresIn = expiresIn,
                    Role = role
                });
            }
            catch (Exception ex)
            {
                return Ok(new LoginResponseDto
                {
                    Success = false,
                    Message = "SERVER ERROR: " + ex.Message
                });
            }
        }


        [HttpPost("change-password")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var result = await authService.ChangePasswordAsync(userId, dto);

            if (!result.success)
                return BadRequest(result.message);

            return Ok(new { message = result.message });
        }


    }
}