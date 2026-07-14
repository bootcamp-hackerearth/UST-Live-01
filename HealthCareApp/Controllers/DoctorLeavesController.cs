using HealthCareApp.Services;
using HealthCareApp.Shared.Dtos.DoctorLeaves;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthCareApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorLeavesController : ControllerBase
    {
        private const string InvalidUserTokenMessage = "Invalid user token.";

        private readonly IDoctorLeaveService _doctorLeaveService;

        public DoctorLeavesController(
            IDoctorLeaveService doctorLeaveService)
        {
            _doctorLeaveService = doctorLeaveService;
        }

        [HttpPost("my")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Doctor")]
        public async Task<IActionResult> CreateMyLeave(
            [FromBody] CreateDoctorLeaveRequest request)
        {
            var identityUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(identityUserId))
            {
                return Unauthorized(new
                {
                    Message = InvalidUserTokenMessage
                });
            }

            var leave = await _doctorLeaveService.CreateMyLeaveAsync(
                identityUserId,
                request);

            return Ok(leave);
        }

        [HttpGet("my")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Doctor")]
        public async Task<IActionResult> GetMyLeaves()
        {
            var identityUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(identityUserId))
            {
                return Unauthorized(new
                {
                    Message = InvalidUserTokenMessage
                });
            }

            var leaves = await _doctorLeaveService.GetMyLeavesAsync(identityUserId);

            return Ok(leaves);
        }

        [HttpGet("doctor/{doctorId:int}")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin")]
        public async Task<IActionResult> GetDoctorLeaves(
            [FromRoute] int doctorId)
        {
            var leaves = await _doctorLeaveService.GetDoctorLeavesAsync(doctorId);

            return Ok(leaves);
        }

        [HttpGet("doctor/{doctorId:int}/status")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin")]
        public async Task<IActionResult> GetDoctorLeaveStatus(
            [FromRoute] int doctorId,
            [FromQuery] DateOnly date)
        {
            var status = await _doctorLeaveService.GetDoctorLeaveStatusAsync(
                doctorId,
                date);

            return Ok(status);
        }
    }
}