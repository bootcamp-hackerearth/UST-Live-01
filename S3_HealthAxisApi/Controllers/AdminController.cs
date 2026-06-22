using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using S3_HealthAxisApi.Services.Interface;
using S3_HealthAxis.Shared.DTOs;
using S3_HealthAxis.Shared.Enums;

namespace S3_HealthAxisApi.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(
            IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult>
            GetDashboard()
        {
            return Ok(
                await _adminService
                    .GetDashboardAsync());
        }

        [HttpGet("statistics")]
        public async Task<IActionResult>
            GetStatistics()
        {
            return Ok(
                await _adminService
                    .GetStatisticsAsync());
        }

        [HttpGet("users")]
        public async Task<IActionResult>
            GetUsers()
        {
            return Ok(
                await _adminService
                    .GetUsersAsync());
        }

        [HttpGet("users/{id}")]
        public async Task<IActionResult>
            GetUser(int id)
        {
            var user =
                await _adminService
                    .GetUserByIdAsync(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }
    }
}