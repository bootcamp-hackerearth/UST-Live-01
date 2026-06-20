using HealthAxisCore_Api.Models.Dtos;
using HealthAxisCore_Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxisCore_Api.Controllers
{
    [ApiController]
    [Route("api/health-records")]
    [Authorize(Roles = "Patient,Doctor")]
    public class HealthRecordController(IHealthRecordService service) : ControllerBase
    {
        [HttpGet("{patientId:int}")]
        public async Task<ActionResult<List<HealthRecordDto>>> GetByPatientId(
            int patientId,
            CancellationToken ct)
        {
            return Ok(await service.GetByPatientIdAsync(patientId, User, ct));
        }

        [HttpGet("details/{id:int}")]
        public async Task<ActionResult<HealthRecordDto>> GetById(
            int id,
            CancellationToken ct)
        {
            return Ok(await service.GetByIdAsync(id, User, ct));
        }

        [HttpPost]
        [Authorize(Roles = "Doctor")]
        public async Task<ActionResult<HealthRecordDto>> Create(
            CreateHealthRecordDto request,
            CancellationToken ct)
        {
            return Ok(await service.CreateAsync(request, User, ct));
        }
    }
}