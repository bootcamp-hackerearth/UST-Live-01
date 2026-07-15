using HealthAxis.Shared.DTOs.DoctorLeaves;
using HealthAxisCore_Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthAxisCore_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Doctor")]
    public class DoctorLeavesController : ControllerBase
    {
        private readonly IDoctorLeaveService _doctorLeaveService;

        public DoctorLeavesController(
            IDoctorLeaveService doctorLeaveService)
        {
            _doctorLeaveService = doctorLeaveService;
        }

        [HttpPost]
        public async Task<ActionResult<DoctorLeaveDto>> CreateLeave(
            CreateMyDoctorLeaveDto dto)
        {
            var doctorIdClaim =
                User.FindFirst("ReferenceId")?.Value;

            if (string.IsNullOrWhiteSpace(doctorIdClaim))
            {
                return Unauthorized();
            }

            var doctorId = int.Parse(doctorIdClaim);

            var result =
                await _doctorLeaveService.CreateLeaveAsync(
                    doctorId,
                    dto);

            return Ok(result);
        }

        [HttpGet("my")]
        public async Task<ActionResult<List<DoctorLeaveDto>>> GetMyLeaves()
        {
            var doctorIdClaim =
                User.FindFirst("ReferenceId")?.Value;

            if (string.IsNullOrWhiteSpace(doctorIdClaim))
            {
                return Unauthorized();
            }

            var doctorId = int.Parse(doctorIdClaim);

            var result =
                await _doctorLeaveService.GetDoctorLeavesAsync(
                    doctorId);

            return Ok(result);
        }
    }
}