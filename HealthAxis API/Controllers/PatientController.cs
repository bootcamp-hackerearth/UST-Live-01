using HealthAxis.API.DTOs.Patients;
using HealthAxis.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxis.API.Controllers
{
    [Route("api/patients")]
    [ApiController]
    [Authorize(Roles = "Patient,Admin")]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(
            int id,
            CancellationToken ct)
        {
            PatientReadDto? patient =
                await _patientService.GetByIdAsync(id, ct);

            if (patient == null)
            {
                return NotFound(new { message = "Patient not found." });
            }

            return Ok(patient);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            PatientUpdateDto request,
            CancellationToken ct)
        {
            PatientReadDto? patient =
                await _patientService.UpdateAsync(id, request, ct);

            if (patient == null)
            {
                return NotFound(new { message = "Patient not found." });
            }

            return Ok(patient);
        }

        [HttpGet("{id:int}/health-records")]
        public async Task<IActionResult> GetHealthRecords(
            int id,
            CancellationToken ct)
        {
            var records =
                await _patientService.GetHealthRecordsAsync(id, ct);

            return Ok(records);
        }
    }
}
