using HealthAxis.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthAxis.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        private string? GetLoggedInUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        [HttpGet]
        [Authorize(Roles = "Patient,Admin,Doctor")]
        public async Task<IActionResult> GetAllDoctors()
        {
            var doctors = await _doctorService.GetAllAsync();

            return Ok(doctors);
        }

        [HttpGet("me")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetMyDoctorProfile()
        {
            var userId = GetLoggedInUserId();

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized(new
                {
                    message = "Invalid token"
                });
            }

            var doctor = await _doctorService.GetByUserIdAsync(userId);

            return Ok(doctor);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Patient,Doctor,Admin")]
        public async Task<IActionResult> GetDoctorById(int id)
        {
            if (User.IsInRole("Doctor"))
            {
                var userId = GetLoggedInUserId();

                if (string.IsNullOrWhiteSpace(userId))
                {
                    return Unauthorized(new
                    {
                        message = "Invalid token"
                    });
                }

                var loggedInDoctor = await _doctorService.GetByUserIdAsync(userId);

                if (loggedInDoctor == null)
                {
                    return NotFound(new
                    {
                        message = "Doctor profile not found"
                    });
                }

                if (loggedInDoctor.DoctorId != id)
                {
                    return StatusCode(403, new
                    {
                        message = "You are not allowed to access another doctor's details"
                    });
                }

                return Ok(loggedInDoctor);
            }

            var doctor = await _doctorService.GetByIdAsync(id);

            return Ok(doctor);
        }

        [HttpGet("{id}/availability")]
        [Authorize(Roles = "Patient,Admin")]
        public async Task<IActionResult> GetDoctorAvailability(int id)
        {
            var doctor = await _doctorService.GetAvailabilityAsync(id);

            return Ok(new
            {
                doctor.DoctorId,
                doctor.FullName,
                doctor.IsActive,
                Message = doctor.IsActive ? "Doctor is available" : "Doctor is not available"
            });
        }
    }
}