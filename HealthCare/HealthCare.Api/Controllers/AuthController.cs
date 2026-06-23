using HealthCare.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Healthcare.Shared.DTOs.Authentication;
using Healthcare.Shared.DTOs.Patient;


namespace HealthCare.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        //  PATIENT SELF-REGISTRATION
        [HttpPost("register-patient")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterPatient([FromBody] CreatePatientDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _authService.RegisterPatientAsync(dto);

            return Ok(new { message = "Patient registered successfully" });
        }

        //  ADMIN CREATES DOCTOR
        [HttpPost("register-doctor")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> RegisterDoctor([FromBody] DoctorRegisterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _authService.RegisterDoctorAsync(dto);

            return Ok(new { message = "Doctor created successfully" });
        }

        //  LOGIN (PATIENT / DOCTOR / ADMIN)
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _authService.LoginAsync(dto);

            return Ok(response);
        }

        [HttpPost("change-password")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            await _authService.ChangePasswordAsync(userId!, dto);

            return Ok("Password changed successfully");
        }
    }
}