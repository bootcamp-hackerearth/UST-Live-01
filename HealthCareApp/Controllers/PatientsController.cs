using HealthCareApp.Dtos;
using HealthCareApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthCareApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

        // Admin only: view all patients.
        [HttpGet]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin")]
        public async Task<IActionResult> GetAllPatients()
        {
            var patients = await _patientService.GetAllPatientsAsync();

            return Ok(patients);
        }

        // Patient only: view own profile.
        // Uses logged-in user's IdentityUserId from JWT token.
        [HttpGet("me")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Patient")]
        public async Task<IActionResult> GetMyProfile()
        {
            var identityUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(identityUserId))
            {
                return Unauthorized(new
                {
                    Message = "Invalid user token."
                });
            }

            var patient = await _patientService.GetMyProfileAsync(identityUserId);

            return Ok(patient);
        }

        // Patient only: update own profile.
        // Uses logged-in user's IdentityUserId from JWT token.
        [HttpPut("me")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Patient")]
        public async Task<IActionResult> UpdateMyProfile([FromBody] UpdatePatientDto request)
        {
            var identityUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(identityUserId))
            {
                return Unauthorized(new
                {
                    Message = "Invalid user token."
                });
            }

            var patient = await _patientService.UpdateMyProfileAsync(identityUserId, request);

            return Ok(patient);
        }

        // Patient only: view own health records.
        // Patient cannot change patientId in URL here.
        [HttpGet("me/health-records")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Patient")]
        public async Task<IActionResult> GetMyHealthRecords()
        {
            var identityUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(identityUserId))
            {
                return Unauthorized(new
                {
                    Message = "Invalid user token."
                });
            }

            var patient = await _patientService.GetMyProfileAsync(identityUserId);

            var records = await _healthRecordService.GetHealthRecordsByPatientIdAsync(patient.PatientId);

            return Ok(records);
        }

        // Admin only: view patient by id.
        // Patient should not use this anymore.
        [HttpGet("{patientId:int}")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin")]
        public async Task<IActionResult> GetPatientById([FromRoute] int patientId)
        {
            var patient = await _patientService.GetPatientByIdAsync(patientId);

            return Ok(patient);
        }

        // Admin only: update patient by id.
        // Patient should use PUT /api/Patients/me instead.
        [HttpPut("{patientId:int}")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin")]
        public async Task<IActionResult> UpdatePatient(
            [FromRoute] int patientId,
            [FromBody] UpdatePatientDto request)
        {
            var patient = await _patientService.UpdatePatientAsync(patientId, request);

            return Ok(patient);
        }

        // Admin and Doctor: view patient health records by patient id.
        // Patient should use GET /api/Patients/me/health-records instead.
        [HttpGet("{patientId:int}/health-records")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin,Doctor")]
        public async Task<IActionResult> GetPatientHealthRecords([FromRoute] int patientId)
        {
            var records = await _healthRecordService.GetHealthRecordsByPatientIdAsync(patientId);

            return Ok(records);
        }
    }
}