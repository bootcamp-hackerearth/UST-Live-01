using HealthApp.Api.Dto;
using HealthApp.Api.Model;
using HealthApp.Api.Service.Interface;
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
            var (success, message, userId) = await authService.RegisterPatientAsync(request);

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
            var (success, message, userId, temporaryPassword) = await authService.RegisterDoctorByAdminAsync(request);

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
            var (success, message, userId) = await authService.Register(register);

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
            var (success, message, accessToken, expiresIn) = await authService.Login(login);

            if (!success)
            {
                return BadRequest(message);
            }

            return Ok(new AuthResponse
            {
                AccessToken = accessToken,
                ExpiresIn = expiresIn,
                message = message
            });
        }
    }
}