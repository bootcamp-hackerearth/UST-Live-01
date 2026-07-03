using HealthCareApp.Services;
using HealthCareApp.Shared.Dtos.Pagination;
using HealthCareApp.Shared.Dtos.Patients;
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
        private const string InvalidUserTokenMessage = "Invalid user token.";

        private readonly IPatientService _patientService;
        private readonly IHealthRecordService _healthRecordService;

        public PatientsController(
            IPatientService patientService,
            IHealthRecordService healthRecordService)
        {
            _patientService = patientService;
            _healthRecordService = healthRecordService;
        }

        [HttpGet("{patientId:int}/health-records")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Doctor")]
        public async Task<IActionResult> GetPatientHealthRecords([FromRoute] int patientId)
        {
            var identityUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(identityUserId))
            {
                return Unauthorized(new
                {
                    Message = InvalidUserTokenMessage
                });
            }

            var records = await _healthRecordService.GetPatientHealthRecordsForTreatingDoctorAsync(
                patientId,
                identityUserId);

            return Ok(records);
        }

        [HttpGet]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin")]
        public async Task<IActionResult> GetAllPatients([FromQuery] PatientPaginationQueryDto query)
        {
            var patients = await _patientService.GetAllPatientsPagedAsync(query);

            return Ok(patients);
        }

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
                    Message = InvalidUserTokenMessage
                });
            }

            var patient = await _patientService.GetMyProfileAsync(identityUserId);

            return Ok(patient);
        }

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
                    Message = InvalidUserTokenMessage
                });
            }

            var patient = await _patientService.UpdateMyProfileAsync(identityUserId, request);

            return Ok(patient);
        }

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
                    Message = InvalidUserTokenMessage
                });
            }

            var patient = await _patientService.GetMyProfileAsync(identityUserId);

            var records = await _healthRecordService.GetHealthRecordsByPatientIdAsync(patient.PatientId);

            return Ok(records);
        }

        [HttpGet("{patientId:int}")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin")]
        public async Task<IActionResult> GetPatientById([FromRoute] int patientId)
        {
            var patient = await _patientService.GetPatientByIdAsync(patientId);

            return Ok(patient);
        }

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
    }
}