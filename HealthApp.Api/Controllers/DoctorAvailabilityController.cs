using HealthApp.Api.Exceptions;
using HealthApp.Api.Extensions;
using HealthApp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthApp.Api.Controllers
{
    [ApiController]
    [Route("api/doctors")]
    [Authorize(
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
        Roles = "Patient,Doctor,Admin")]
    public class DoctorAvailabilityController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public DoctorAvailabilityController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet("{id:int}/availability")]
        public async Task<IActionResult> GetDoctorAvailability(
            int id,
            [FromQuery] DateOnly date)
        {
            var slots = await _appointmentService.GetAvailableSlotsAsync(
                id,
                date);

            return Ok(slots);
        }

        [HttpGet("profile/availability")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Doctor")]
        public async Task<IActionResult> GetMyAvailability(
            [FromQuery] DateOnly date)
        {
            var doctorId = User.GetDoctorId();

            if (doctorId == null)
            {
                throw new ForbiddenAccessException(
                    "Doctor profile is not linked to this user.");
            }

            var slots = await _appointmentService.GetAvailableSlotsAsync(
                doctorId.Value,
                date);

            return Ok(slots);
        }
    }
}
