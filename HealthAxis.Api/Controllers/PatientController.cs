using HealthAxisCore_Api.Models.Dtos;
using HealthAxisCore_Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace HealthAxisCore_Api.Controllers
{
    [ApiController]
    [Route("api/patients")]
    [Authorize]
    public class PatientController(IPatientService service) : ControllerBase
    {
        [HttpGet("{id:int}")] public async Task<ActionResult<PatientDto>> GetById(int id, CancellationToken ct) => Ok(await service.GetByIdAsync(id, ct));
        [HttpPut("{id:int}")] [Authorize(Roles="Patient")] public async Task<ActionResult<PatientDto>> Update(int id, UpdatePatientDto request, CancellationToken ct) => Ok(await service.UpdatePatientAsync(id, request, ct));
        [HttpGet("{id:int}/health-records")] public async Task<ActionResult<List<HealthRecordDto>>> GetHealthRecords(int id, CancellationToken ct) => Ok(await service.GetHealthRecordsAsync(id, ct));
    }
}
