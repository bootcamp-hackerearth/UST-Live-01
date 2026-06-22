using HealthAxis.API.Services.Interfaces;
using HealthAxis.Shared.DTOs.Doctor;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxis.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Patient,Doctor,Admin")]
    [ApiController]
    [Route("api/doctors")]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _service;

        public DoctorController(IDoctorService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] PaginationParams paginationParams,
            CancellationToken ct)
        {
            var doctors = await _service.GetAllAsync(paginationParams, ct);
            return Ok(doctors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var doctor = await _service.GetByIdAsync(id, ct);

            if (doctor is null)
            {
                return NotFound("Doctor not found");
            }

            return Ok(doctor);
        }

        [HttpGet("{id}/availability")]
        public async Task<IActionResult> GetAvailability(int id, CancellationToken ct)
        {
            var availability = await _service.GetAvailabilityAsync(id, ct);
            return Ok(availability);
        }
    }
}