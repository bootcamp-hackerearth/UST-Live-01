using HealthCareApp.Enums;
using HealthCareApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthCareApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController(IDoctorService service) : ControllerBase
    {
        // Patients need this to view active doctors before booking.
        // Admin can also view.
        [HttpGet]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Patient,Admin")]
        public async Task<IActionResult> GetAllActiveDoctors()
        {
            var result = await service.GetAllActiveDoctorsAsync();

            return Ok(result);
        }

        // Patients need this to view a selected doctor before booking.
        // Admin can also view.
        [HttpGet("{doctorId:int}")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Patient,Admin")]
        public async Task<IActionResult> GetDoctorById([FromRoute] int doctorId)
        {
            var result = await service.GetDoctorByIdAsync(doctorId);

            return Ok(result);
        }

        // Optional search endpoint.
        // This is not directly in company requirement, but useful for filtering doctors.
        [HttpGet("specialisation/{specialisation}")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Patient,Admin")]
        public async Task<IActionResult> GetActiveDoctorsBySpecialisation(
            [FromRoute] SpecialisationType specialisation)
        {
            var result = await service.GetActiveDoctorsBySpecialisationAsync(specialisation);

            return Ok(result);
        }

        // Company requirement:
        // GET /api/doctors/{id}/availability
        [HttpGet("{doctorId:int}/availability")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Patient,Admin")]
        public async Task<IActionResult> GetDoctorAvailability([FromRoute] int doctorId)
        {
            var result = await service.GetDoctorAvailabilityAsync(doctorId);

            return Ok(result);
        }
    }
}