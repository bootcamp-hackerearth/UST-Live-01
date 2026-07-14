using HealthApp.Api.Service.Interface;
using HealthApp.Shared.Dto;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthApp.Api.Controllers
{
    [Route("api/doctor-leaves")]
    [ApiController]
    public class DoctorLeaveController : ControllerBase
    {
        private readonly IDoctorLeaveService _leaveService;

        public DoctorLeaveController(IDoctorLeaveService leaveService)
        {
            _leaveService = leaveService;
        }

        [HttpPost("my")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Doctor")]
        public async Task<IActionResult> CreateMyLeave(
            [FromBody] DoctorLeaveCreateDto dto)
        {
            var identityUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(identityUserId))
                return Unauthorized();

            var result = await _leaveService
                .CreateMyLeaveAsync(dto, identityUserId);

            return Ok(result);
        }

        [HttpGet("my")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Doctor")]
        public async Task<IActionResult> GetMyLeaves()
        {
            var identityUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(identityUserId))
                return Unauthorized();

            var result = await _leaveService
                .GetMyLeavesAsync(identityUserId);

            return Ok(result);
        }

        [HttpGet("doctor/{doctorId:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin")]
        public async Task<IActionResult> GetDoctorLeaves(int doctorId)
        {
            var result = await _leaveService
                .GetLeavesByDoctorIdAsync(doctorId);

            return Ok(result);
        }

        [HttpGet("doctor/{doctorId:int}/check")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin")]
        public async Task<IActionResult> CheckDoctorLeave(int doctorId,[FromQuery] DateTime date)
        {
            var isOnLeave = await _leaveService
                .IsDoctorOnLeaveAsync(doctorId, date);

            return Ok(new
            {
                doctorId,
                date,
                isOnLeave,
                message = isOnLeave
                    ? "Doctor is on leave for the selected date."
                    : "Doctor is available for the selected date."
            });
        }

        [HttpGet("all")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin")]
        public async Task<IActionResult> GetAllDoctorLeaves
            ([FromQuery] int pageNumber = 1,[FromQuery] int pageSize = 10)
        {
            var (items, totalCount) = await _leaveService.GetAllLeaveDoctorAsync(pageNumber,pageSize);

            return Ok(new
            {
                data = items,
                totalRecords = totalCount,
                pageNumber,
                pageSize,
                totalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            });
        }
    }
}