using HealthAxis.API.DTO;
using HealthAxis.API.Services;
using HealthAxis.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxis.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Patient")]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatientById(int id)
        {
            var patient = await _patientService.GetByIdAsync(id);

            return Ok(patient);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(int id, [FromBody] PatientDto patientDto)
        {
            var updatedPatient = await _patientService.UpdateAsync(id, patientDto);

            return Ok(updatedPatient);
        }

        [HttpGet("{id}/health-records")]
        public async Task<IActionResult> GetPatientHealthRecords(int id)
        {
            var records = await _patientService.GetHealthRecordsByPatientIdAsync(id);

            return Ok(records);
        }
    }
}