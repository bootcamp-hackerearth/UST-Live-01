using HealthAxis.API.Services.Interfaces;
using HealthAxis.Shared.DTOs.Doctor;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        [Authorize(Roles = "Doctor")]
        [HttpPut("me/status")]
        public async Task<IActionResult> UpdateCurrentDoctorStatus(
    UpdateDoctorStatusDto dto,
    CancellationToken ct)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var doctor = await _service.GetByUserIdAsync(userId, ct);

            if (doctor == null)
            {
                return NotFound("Doctor profile not found.");
            }

            var updateDto = new UpdateDoctorDto
            {
                FullName = doctor.FullName,
                Specialisation = doctor.Specialisation,
                YearsOfExperience = doctor.YearsOfExperience,
                ConsultationFee = doctor.ConsultationFee,
                IsActive = dto.IsActive
            };

            var result = await _service.UpdateAsync(doctor.DoctorId, updateDto, ct);

            return Ok(result);
        }
        [Authorize(Roles = "Doctor")]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentDoctor(CancellationToken ct)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var doctor = await _service.GetByUserIdAsync(userId, ct);

            return Ok(doctor);
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
        public async Task<IActionResult> GetAvailability(
    int id,
    [FromQuery] DateTime? date,
    CancellationToken ct)
        {
            var availabilityDate = date ?? DateTime.Today;

            var availability = await _service.GetAvailabilityAsync(id, availabilityDate, ct);

            return Ok(availability);
        }
    }
}