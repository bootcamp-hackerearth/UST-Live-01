using HealthAxisHealth.API.DTOs.DoctorDtos;
using HealthAxisHealth.API.Helpers;
using HealthAxisHealth.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxisHealth.API.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController :
        ControllerBase
    {
        #region Fields

        private readonly IAdminService
            _adminService;

        #endregion

        #region Constructor

        public AdminController(
            IAdminService adminService)
        {
            _adminService =
                adminService;
        }

        #endregion

        #region Doctor Management

        [HttpGet("doctors")]
        public async Task<IActionResult>
            GetDoctors(
                [FromQuery]
                PaginationParams pagination)
        {
            var doctors =
                await _adminService
                    .GetDoctorsAsync(
                        pagination);

            return Ok(doctors);
        }

        [HttpPost("doctors")]
        public async Task<IActionResult>
            CreateDoctor(
                CreateDoctorDto dto)
        {
            int doctorId =
                await _adminService
                    .CreateDoctorAsync(dto);

            return Ok(
                new
                {
                    DoctorId = doctorId,
                    Message =
                        "Doctor created successfully."
                });
        }

        [HttpPut("doctors/{id:int}")]
        public async Task<IActionResult>
            UpdateDoctor(
                int id,
                UpdateDoctorDto dto)
        {
            await _adminService
                .UpdateDoctorAsync(
                    id,
                    dto);

            return Ok(
                new
                {
                    Message =
                        "Doctor updated successfully."
                });
        }

        [HttpGet("users")]
        public async Task<IActionResult>
            GetUsers(
              [FromQuery] PaginationParams pagination,
              [FromQuery] string? role)
        {
            var users =
                await _adminService
                    .GetUsersAsync(
                        pagination,
                        role);

            return Ok(users);
        }

        #endregion

        #region Reports

        [HttpGet("reports/appointments")]
        public async Task<IActionResult>
            GetAppointmentReport()
        {
            var report =
                await _adminService
                    .GetAppointmentReportAsync();

            return Ok(report);
        }

        #endregion
    }
}
