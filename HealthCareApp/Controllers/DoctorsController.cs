using HealthCareApp.Enums;
using HealthCareApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthCareApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController(IDoctorService service) : ControllerBase
    {
        // Doctor only: View logged-in doctor's own profile.
        [HttpGet("me")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Doctor")]
        public async Task<IActionResult> GetMyProfile()
        {
            var identityUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(identityUserId))
            {
                return Unauthorized(new
                {
                    Message = "Invalid user token."
                });
            }

            var result = await service.GetMyProfileAsync(identityUserId);

            return Ok(result);
        }

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