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
        private readonly IHealthRecordService _healthRecordService;

        public PatientsController(
            IPatientService patientService,
            IHealthRecordService healthRecordService)
        {
            _patientService = patientService;
            _healthRecordService = healthRecordService;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(
            int id,
            CancellationToken ct)
        {
            bool isAdmin =
                User.IsInRole("Admin");

            int loggedInReferenceId =
                GetReferenceIdFromToken();

            if (!isAdmin && loggedInReferenceId != id)
            {
                return Forbid();
            }

            PatientReadDto? patient =
                await _patientService.GetByIdAsync(
                    id,
                    ct);

            if (patient == null)
            {
                return NotFound(
                    new
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
            bool isAdmin =
                User.IsInRole("Admin");

            int loggedInReferenceId =
                GetReferenceIdFromToken();

            if (!isAdmin && loggedInReferenceId != id)
            {
                return Forbid();
            }

            PatientReadDto? patient =
                await _patientService.UpdateAsync(
                    id,
                    request,
                    ct);

            if (patient == null)
            {
                return NotFound(
                    new
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
            bool isAdmin =
                User.IsInRole("Admin");

            int loggedInReferenceId =
                GetReferenceIdFromToken();

            if (!isAdmin && loggedInReferenceId != id)
            {
                return Forbid();
            }

            var records =
                await _healthRecordService.GetByPatientIdAsync(
                    id,
                    ct);

            return Ok(records);
        }

        [HttpGet("me/health-records")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Patient")]
        public async Task<IActionResult> GetMyHealthRecords(
            CancellationToken ct)
        {
            int patientId =
                GetReferenceIdFromToken();

            var records =
                await _healthRecordService.GetByPatientIdAsync(
                    patientId,
                    ct);

            return Ok(records);
        }

        private int GetReferenceIdFromToken()
        {
            string? referenceIdClaim =
                User.FindFirst("ReferenceId")?.Value;

            if (string.IsNullOrWhiteSpace(referenceIdClaim) ||
                !int.TryParse(referenceIdClaim, out int referenceId))
            {
                throw new UnauthorizedAccessException(
                    "Reference Id was not found in token.");
            }

            return referenceId;
        }
    }
}
