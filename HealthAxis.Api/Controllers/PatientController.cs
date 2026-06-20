using HealthAxisCore_Api.Models.Dtos;
using HealthAxisCore_Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxisCore_Api.Controllers
{
    [ApiController]
    [Route("api/patients")]
    [Authorize(Roles = "Patient,Doctor,Admin")]
    public class PatientController(IPatientService service) : ControllerBase
    {
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<List<PatientDto>>> GetAll(
            CancellationToken ct)
        {
            return Ok(await service.GetAllAsync(User, ct));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PatientDto>> GetById(
            int id,
            CancellationToken ct)
        {
            return Ok(await service.GetByIdAsync(id, User, ct));
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Patient")]
        public async Task<ActionResult<PatientDto>> Update(
            int id,
            UpdatePatientDto request,
            CancellationToken ct)
        {
            return Ok(await service.UpdatePatientAsync(id, request, User, ct));
        }

        [HttpGet("{id:int}/health-records")]
        [Authorize(Roles = "Patient,Doctor")]
        public async Task<ActionResult<List<HealthRecordDto>>> GetHealthRecords(
            int id,
            CancellationToken ct)
        {
            return Ok(await service.GetHealthRecordsAsync(id, User, ct));
        }
    }
}