using HealthApp.Api.Enums;
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
    [Authorize]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorService _doctorService;
        private readonly IAppointmentService _appointmentService;

        public DoctorsController(
            IDoctorService doctorService,
            IAppointmentService appointmentService)
        {
            _doctorService = doctorService;
            _appointmentService = appointmentService;
        }

        [HttpGet("profile")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Doctor")]
        public async Task<IActionResult> GetMyProfile()
        {
            var doctorId = User.GetDoctorId();

            if (doctorId == null)
            {
                throw new ForbiddenAccessException(
                    "Doctor profile is not linked to this user.");
            }

            var doctor = await _doctorService.GetDoctorByIdAsync(doctorId.Value);

            return Ok(doctor);
        }

        [HttpGet("profile/availability")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Doctor")]
        public async Task<IActionResult> GetMyAvailability([FromQuery] DateOnly date)
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

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Patient,Doctor,Admin")]
        public async Task<IActionResult> GetDoctors(
            [FromQuery] string? search,
            [FromQuery] SpecialisationType? specialisation,
            [FromQuery] bool? isActive = true)
        {
            var doctors = await _doctorService.SearchDoctorsAsync(
                search,
                specialisation,
                isActive);

            return Ok(doctors);
        }

        [HttpGet("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Patient,Doctor,Admin")]
        public async Task<IActionResult> GetDoctorById(int id)
        {
            var doctor = await _doctorService.GetDoctorByIdAsync(id);

            return Ok(doctor);
        }

        [HttpGet("specialisation/{specialisation}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Patient,Doctor,Admin")]
        public async Task<IActionResult> GetDoctorsBySpecialisation(
            string specialisation)
        {
            if (!Enum.TryParse(
                    specialisation,
                    true,
                    out SpecialisationType specialisationType))
            {
                throw new InvalidRequestException("Invalid specialisation.");
            }

            var doctors = await _doctorService.GetDoctorsBySpecialisationAsync(
                specialisationType);

            return Ok(doctors);
        }

        [HttpGet("{id:int}/availability")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Patient,Doctor,Admin")]
        public async Task<IActionResult> GetDoctorAvailability(
            int id,
            [FromQuery] DateOnly date)
        {
            var slots = await _appointmentService.GetAvailableSlotsAsync(id, date);

            return Ok(slots);
        }
    }
}