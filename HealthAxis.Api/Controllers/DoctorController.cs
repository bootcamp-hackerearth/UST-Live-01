using HealthAxisCore_Api.Models.Dtos;
using HealthAxisCore_Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace HealthAxisCore_Api.Controllers
{
    [ApiController]
    [Route("api/doctors")]
    [Authorize]
    public class DoctorController(IDoctorService service) : ControllerBase
    {
        [HttpGet] [Authorize(Roles="Patient,Admin")] public async Task<ActionResult<List<DoctorDto>>> GetDoctors([FromQuery] string? specialisation, CancellationToken ct) => Ok(await service.GetDoctorsAsync(specialisation, ct));
        [HttpGet("{id:int}")] public async Task<ActionResult<DoctorDto>> GetById(int id, CancellationToken ct) => Ok(await service.GetByIdAsync(id, ct));
        [HttpGet("{id:int}/availability")] [Authorize(Roles="Patient")] public async Task<ActionResult<List<string>>> GetAvailability(int id, [FromQuery] DateTime date, CancellationToken ct) => Ok(await service.GetAvailabilityAsync(id, date, ct));
    }
}
