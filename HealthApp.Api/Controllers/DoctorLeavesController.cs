using HealthApp.Api.Exceptions;
using HealthApp.Api.Extensions;
using HealthApp.Api.Services.Interfaces;
using HealthApp.Shared.Dtos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthApp.Api.Controllers
{
    [ApiController]
    [Route("api/doctors/leaves")]
    [Authorize(
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
        Roles = "Doctor")]
    public class DoctorLeavesController : ControllerBase
    {
        private readonly IDoctorLeaveService _doctorLeaveService;

        public DoctorLeavesController(
            IDoctorLeaveService doctorLeaveService)
        {
            _doctorLeaveService = doctorLeaveService;
        }

        [HttpPost("preview")]
        public async Task<IActionResult> PreviewMyLeave(
            [FromBody] DoctorLeaveCreateDto dto,
            CancellationToken ct)
        {
            var doctorId = GetRequiredDoctorId();

            var preview = await _doctorLeaveService.PreviewLeaveAsync(
                doctorId,
                dto,
                ct);

            return Ok(preview);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMyLeave(
            [FromBody] DoctorLeaveCreateDto dto,
            CancellationToken ct)
        {
            var doctorId = GetRequiredDoctorId();

            var result = await _doctorLeaveService.CreateLeaveAsync(
                doctorId,
                dto,
                ct);

            var message = result.CancelledAppointmentCount > 0
                ? $"Doctor leave created successfully. " +
                  $"{result.CancelledAppointmentCount} affected appointment(s) were cancelled."
                : "Doctor leave created successfully.";

            return Ok(new
            {
                message,
                data = result
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetMyLeaves(
            CancellationToken ct)
        {
            var doctorId = GetRequiredDoctorId();

            var leaves = await _doctorLeaveService.GetDoctorLeavesAsync(
                doctorId,
                ct);

            return Ok(leaves);
        }

        private int GetRequiredDoctorId()
        {
            var doctorId = User.GetDoctorId();

            if (doctorId == null)
            {
                throw new ForbiddenAccessException(
                    "Doctor profile is not linked to this user.");
            }

            return doctorId.Value;
        }
    }
}
