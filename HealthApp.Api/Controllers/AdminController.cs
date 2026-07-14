using HealthApp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthApp.Api.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
        Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers(
            [FromQuery] string? role)
        {
            var users = await _adminService.GetUsersAsync(role);
            return Ok(users);
        }

        [HttpGet("reports/appointments")]
        public async Task<IActionResult> GetAppointmentReports()
        {
            var reports = await _adminService
                .GetAppointmentReportsAsync();

            return Ok(reports);
        }

        [HttpGet("doctor-leaves")]
        public async Task<IActionResult> GetDoctorLeaves(
            [FromQuery] string? search,
            [FromQuery] string? status,
            [FromQuery] DateOnly? fromDate,
            [FromQuery] DateOnly? toDate,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken ct = default)
        {
            var leaves = await _adminService.GetDoctorLeavesAsync(
                search,
                status,
                fromDate,
                toDate,
                pageNumber,
                pageSize,
                ct);

            return Ok(leaves);
        }
    }
}