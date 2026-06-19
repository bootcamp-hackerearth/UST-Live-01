using HealthCare.Api.DTOs.Authentication;
using HealthCare.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthCare.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IAuthService _authService;

        public RegisterController(IAuthService authService)
        {
            _authService = authService;
        }

        //  PATIENT SELF-REGISTRATION
        [AllowAnonymous]

        [HttpPost("register-patient")]
        public async Task<IActionResult> RegisterPatient([FromBody] PatientRegisterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _authService.RegisterPatientAsync(dto);

            return Ok(new { message = "Patient registered successfully" });
        }

        //  ADMIN CREATES DOCTOR
        [HttpPost("register-doctor")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin")]
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

        //  GET CURRENT USER INFO (FROM TOKEN)
        [Authorize]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("me")]
        public IActionResult GetCurrentUser()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            var patientId = User.FindFirst("PatientId")?.Value;
            var doctorId = User.FindFirst("DoctorId")?.Value;

            return Ok(new
            {
                userId,
                email,
                role,
                patientId,
                doctorId
            });
        }
    }
}