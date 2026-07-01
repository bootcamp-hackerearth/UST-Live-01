using HealthApp.Api.Exceptions;
using HealthApp.Api.Extensions;
using HealthApp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthApp.Api.Controllers
{
    [ApiController]
    [Route("api/doctors/profile")]
    [Authorize(
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
        Roles = "Doctor")]
    public class DoctorProfileController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorProfileController(
            IDoctorService doctorService,
            IAppointmentService appointmentService)
        {
            _doctorService = doctorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyProfile()
        {
            var doctorId = User.GetDoctorId();

            if (doctorId == null)
            {
                throw new ForbiddenAccessException(
                    "Doctor profile is not linked to this user.");
            }

            var doctor = await _doctorService.GetDoctorByIdAsync(
                doctorId.Value);

            return Ok(doctor);
        }

        [HttpPatch("status")]
        public async Task<IActionResult> ChangeMyStatus([FromQuery] bool isActive)
        {
            var doctorId = User.GetDoctorId();

            if (doctorId == null)
            {
                throw new ForbiddenAccessException(
                    "Doctor profile is not linked to this user.");
            }

            await _doctorService.ChangeStatusAsync(doctorId.Value, isActive);

            return Ok(new
            {
                message = isActive
                    ? "You are now available for appointments."
                    : "You are now unavailable for appointments."
            });
        }
    }
}