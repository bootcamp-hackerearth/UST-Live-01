using HealthApp.Api.Enums;
using HealthApp.Api.Exceptions;
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
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorsController(
            IDoctorService doctorService,
            IAppointmentService appointmentService)
        {
            _doctorService = doctorService;
        }

        [HttpGet]
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
        public async Task<IActionResult> GetDoctorById(int id)
        {
            var doctor = await _doctorService.GetDoctorByIdAsync(id);

            return Ok(doctor);
        }

        [HttpGet("specialisation/{specialisation}")]
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
    }
}