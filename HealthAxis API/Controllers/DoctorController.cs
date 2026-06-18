using HealthAxis.API.DTOs.Doctors;
using HealthAxis.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxis.API.Controllers
{
    [Route("api/doctors")]
    [ApiController]
    [Authorize]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorsController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet]
        [Authorize(Roles = "Patient,Doctor,Admin")]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var doctors = await _doctorService.GetAllAsync(ct);

            return Ok(doctors);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Patient,Doctor,Admin")]
        public async Task<IActionResult> GetById(
            int id,
            CancellationToken ct)
        {
            var doctor = await _doctorService.GetByIdAsync(id, ct);

            if (doctor == null)
            {
                return NotFound(new { message = "Doctor not found." });
            }

            return Ok(doctor);
        }

        [HttpGet("{id:int}/availability")]
        [Authorize(Roles = "Patient,Admin")]
        public async Task<IActionResult> GetAvailability(
            int id,
            [FromQuery] DateTime date,
            [FromQuery] string timeSlot,
            CancellationToken ct)
        {
            DoctorAvailabilityDto? availability =
                await _doctorService.GetAvailabilityAsync(id, date, timeSlot, ct);

            if (availability == null)
            {
                return NotFound(new { message = "Doctor not found." });
            }

            return Ok(availability);
        }
    }
}
