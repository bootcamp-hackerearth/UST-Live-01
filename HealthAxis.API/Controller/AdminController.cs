using HealthAxis.API.DTO.DoctorDtos;
using HealthAxis.API.Services.Interfaces;
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

        [HttpGet("doctors")]
        public async Task<IActionResult> GetAllDoctors()
        {
            var doctors =
                await _adminService.GetAllDoctorsAsync();

            return Ok(doctors);
        }

        [HttpPost("doctors")]
        public async Task<IActionResult> AddDoctor(
            [FromBody] DoctorDto doctorDto)
        {
            var doctor =
                await _adminService.AddDoctorAsync(doctorDto);

            return Ok(doctor);
        }

        [HttpPut("doctors/{id}")]
        public async Task<IActionResult> UpdateDoctor(
            int id,
            [FromBody] DoctorDto doctorDto)
        {
            var doctor =
                await _adminService.UpdateDoctorAsync(id, doctorDto);

            return Ok(doctor);
        }

        [HttpGet("reports/appointments")]
        public async Task<IActionResult> GetAppointmentReports()
        {
            var reports =
                await _adminService.GetAppointmentReportsAsync();

            return Ok(reports);
        }
    }
}