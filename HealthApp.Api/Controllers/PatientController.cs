using HealthApp.Api.Dtos;
using HealthApp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HealthApp.Api.Controllers
{
    [ApiController]
    [Route("api/patients")]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPatients()
        {
            var patients = await _patientService.GetAllPatientsAsync();

            return Ok(patients);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetPatientById(int id)
        {
            var patient = await _patientService.GetPatientByIdAsync(id);

            return Ok(patient);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePatient([FromBody] PatientCreateDto dto)
        {
            await _patientService.RegisterPatientAsync(dto);

            return StatusCode(
                StatusCodes.Status201Created,
                new { message = "Patient created successfully." });
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdatePatient(
            int id,
            [FromBody] PatientCreateDto dto)
        {
            await _patientService.UpdatePatientAsync(id, dto);

            return Ok(new { message = "Patient updated successfully." });
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchPatients(
            [FromQuery] string? name,
            [FromQuery] string? email)
        {
            var patients = await _patientService.SearchPatientsAsync(name, email);

            return Ok(patients);
        }
    }
}