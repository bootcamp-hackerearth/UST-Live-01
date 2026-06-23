using HealthAxis.API.Services.Interfaces;
using HealthAxis.Shared.DTO.DoctorDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxis.API.Controller
{
    [Route("api/admin")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("doctors")]
        public async Task<IActionResult> AddDoctor(
            [FromBody] CreateDoctorDto doctorDto)
        {
            var doctor = await _adminService.AddDoctorAsync(doctorDto);

            return Ok(doctor);
        }

        [HttpGet("doctors")]
        public async Task<IActionResult> GetAllDoctors()
        {
            var doctors = await _adminService.GetAllDoctorsAsync();

            return Ok(doctors);
        }

        [HttpPut("doctors/{id}")]
        public async Task<IActionResult> UpdateDoctor(
            int id,
            [FromBody] UpdateDoctorDto doctorDto)
        {
            var doctor = await _adminService.UpdateDoctorAsync(id, doctorDto);

            return Ok(doctor);
        }

        [HttpGet("reports/appointments")]
        public async Task<IActionResult> GetAppointmentReports()
        {
            var reports = await _adminService.GetAppointmentReportsAsync();

            return Ok(reports);
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _adminService.GetUsersAsync();

            return Ok(users);
        }
    }
}