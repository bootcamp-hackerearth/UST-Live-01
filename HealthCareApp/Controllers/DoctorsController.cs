using HealthCareApp.Services;
using HealthCareApp.Shared.Enums;
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
        private const string InvalidUserTokenMessage = "Invalid user token.";

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
                    Message = InvalidUserTokenMessage
                });
            }

            var result = await service.GetMyProfileAsync(identityUserId);

            return Ok(result);
        }

        [HttpGet]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Patient,Admin")]
        public async Task<IActionResult> GetAllActiveDoctors()
        {
            var result = await service.GetAllActiveDoctorsAsync();

            return Ok(result);
        }

        [HttpGet("{doctorId:int}")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Patient,Admin")]
        public async Task<IActionResult> GetDoctorById([FromRoute] int doctorId)
        {
            var result = await service.GetDoctorByIdAsync(doctorId);

            return Ok(result);
        }

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

        [HttpGet("{doctorId:int}/availability")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Patient,Admin")]
        public async Task<IActionResult> GetDoctorAvailability(
            [FromRoute] int doctorId,
            [FromQuery] DateTime? date)
        {
            var result = await service.GetDoctorAvailabilityAsync(
                doctorId,
                date);

            return Ok(result);
        }
    }
}