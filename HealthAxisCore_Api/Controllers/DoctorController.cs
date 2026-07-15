using HealthAxisCore_Api.Models.Dtos;
using HealthAxisCore_Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxisCore_Api.Controllers
{
    [ApiController]
    [Route("api/doctors")]
    [Authorize(Roles = "Patient,Doctor,Admin")]
    public class DoctorController(IDoctorService service) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<DoctorDto>>> GetDoctors(
            [FromQuery] string? specialisation,
            CancellationToken ct)
        {
            return Ok(await service.GetDoctorsAsync(specialisation, User, ct));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<DoctorDto>> GetById(
            int id,
            CancellationToken ct)
        {
            return Ok(await service.GetByIdAsync(id, User, ct));
        }

        [HttpGet("{id:int}/availability")]
        [Authorize(Roles = "Patient,Admin")]
        public async Task<ActionResult<List<string>>> GetAvailability(
            int id,
            [FromQuery] DateTime date,
            CancellationToken ct)
        {
            return Ok(await service.GetAvailabilityAsync(id, date, ct));
        }

        [HttpPut("{id:int}/status")]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<DoctorDto>> UpdateOwnStatus(
    int id,
    [FromQuery] bool isActive,
    CancellationToken ct)
        {
            return Ok(await service.UpdateOwnStatusAsync(
                id,
                isActive,
                User,
                ct));
        }
    }
}