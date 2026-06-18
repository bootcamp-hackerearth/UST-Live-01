using HealthApp.Api.Dtos;
using HealthApp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthApp.Api.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IDoctorService _doctorService;
        private readonly IAuthService _authService;
        private readonly IAdminService _adminService;

        public AdminController(
            IDoctorService doctorService,
            IAuthService authService,
            IAdminService adminService)
        {
            _doctorService = doctorService;
            _authService = authService;
            _adminService = adminService;
        }

        [HttpGet("doctors")]
        public async Task<IActionResult> GetDoctors()
        {
            var doctors = await _doctorService.GetAllDoctorsAsync();

            return Ok(doctors);
        }

        [HttpPost("doctors")]
        public async Task<IActionResult> CreateDoctor([FromBody] RegisterDoctorDto dto)
        {
            var result = await _authService.RegisterDoctor(dto);

            return StatusCode(StatusCodes.Status201Created, new
            {
                result.message,
                result.userId,
                result.temporaryPassword
            });
        }

        [HttpPut("doctor/{id:int}")]
        public async Task<IActionResult> UpdateDoctor(
            int id,
            [FromBody] DoctorCreateDto dto)
        {
            await _doctorService.UpdateDoctorAsync(id, dto);

            return Ok(new { message = "Doctor updated successfully." });
        }

        [HttpPatch("doctor/{id:int}/status")]
        public async Task<IActionResult> ChangeDoctorStatus(
            int id,
            [FromQuery] bool isActive)
        {
            await _doctorService.ChangeStatusAsync(id, isActive);

            return Ok(new { message = "Doctor status updated successfully." });
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers([FromQuery] string? role)
        {
            var users = await _adminService.GetUsersAsync(role);

            return Ok(users);
        }

        [HttpGet("reports/appointments")]
        public async Task<IActionResult> GetAppointmentReports()
        {
            var reports = await _adminService.GetAppointmentReportsAsync();

            return Ok(reports);
        }
    }
}