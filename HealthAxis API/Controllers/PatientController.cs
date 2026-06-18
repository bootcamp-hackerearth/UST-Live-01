using HealthAxis.API.DTOs.Patients;
using HealthAxis.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxis.API.Controllers
{
    [Route("api/patients")]
    [ApiController]
    [Authorize(
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
        Roles = "Patient,Admin")]
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
            bool isAdmin =
                User.IsInRole("Admin");

            string? referenceIdClaim =
                User.FindFirst("ReferenceId")?.Value;

            int.TryParse(referenceIdClaim, out int loggedInReferenceId);

            if (!isAdmin && loggedInReferenceId != id)
            {
                return Forbid();
            }

            PatientReadDto? patient =
                await _patientService.GetByIdAsync(id, ct);

            if (patient == null)
            {
                return NotFound(new
                {
                    message = "Patient not found."
                });
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
                return NotFound(new
                {
                    message = "Patient not found."
                });
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
