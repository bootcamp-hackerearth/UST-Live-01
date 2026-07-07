using HealthAxis.API.Services;
using HealthAxis.API.Services.Interfaces;
using HealthAxis.Shared.DTO.PatientDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthAxis.API.Controller
{
    [Route("api/patients")]
    [ApiController]
    [Authorize(Roles = "Patient")]
    public class PatientController : ControllerBase
    {
        private const string InvalidToken = "Invalid token";
        private const string ProfileNotFound = "Patient profile not found";

        private readonly IPatientService _patientService;
        private readonly IHealthRecordService _healthRecordService;

        public PatientController(
            IPatientService patientService,
            IHealthRecordService healthRecordService)
        {
            _patientService = patientService;
            _healthRecordService = healthRecordService;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = GetLoggedInUserId();

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized(new
                {
                    message = InvalidToken
                });
            }

            var patient = await _patientService.GetByUserIdAsync(userId);

            if (patient == null)
            {
                return NotFound(new
                {
                    message = ProfileNotFound
                });
            }

            return Ok(patient);
        }

        [HttpPut("me")]
        public async Task<IActionResult> UpdateMyProfile(
            [FromBody] UpdatePatientDto patientDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetLoggedInUserId();

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized(new
                {
                    message = InvalidToken
                });
            }

            var loggedInPatient = await _patientService.GetByUserIdAsync(userId);

            if (loggedInPatient == null)
            {
                return NotFound(new
                {
                    message = ProfileNotFound
                });
            }

            var updatedPatient = await _patientService.UpdateAsync(
                loggedInPatient.PatientId,
                patientDto);

            return Ok(updatedPatient);
        }

        [HttpGet("me/health-records")]
        public async Task<IActionResult> GetMyHealthRecords()
        {
            var userId = GetLoggedInUserId();

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized(new
                {
                    message = InvalidToken
                });
            }

            var loggedInPatient = await _patientService.GetByUserIdAsync(userId);

            if (loggedInPatient == null)
            {
                return NotFound(new
                {
                    message = ProfileNotFound
                });
            }

            var records = await _healthRecordService.GetByPatientIdAsync(
                loggedInPatient.PatientId);

            return Ok(records);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatientById(int id)
        {
            var userId = GetLoggedInUserId();

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized(new
                {
                    message = InvalidToken
                });
            }

            var loggedInPatient = await _patientService.GetByUserIdAsync(userId);

            if (loggedInPatient == null)
            {
                return NotFound(new
                {
                    message = ProfileNotFound
                });
            }

            if (loggedInPatient.PatientId != id)
            {
                return StatusCode(403, new
                {
                    message = "You are not allowed to access another patient's details"
                });
            }

            return Ok(loggedInPatient);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(
            int id,
            [FromBody] UpdatePatientDto patientDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetLoggedInUserId();

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized(new
                {
                    message = InvalidToken
                });
            }

            var loggedInPatient = await _patientService.GetByUserIdAsync(userId);

            if (loggedInPatient == null)
            {
                return NotFound(new
                {
                    message = ProfileNotFound
                });
            }

            if (loggedInPatient.PatientId != id)
            {
                return StatusCode(403, new
                {
                    message = "You cannot update another patient's details"
                });
            }

            var updatedPatient = await _patientService.UpdateAsync(
                id,
                patientDto);

            return Ok(updatedPatient);
        }

        [HttpGet("{id}/health-records")]
        public async Task<IActionResult> GetPatientHealthRecords(int id)
        {
            var userId = GetLoggedInUserId();

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized(new
                {
                    message = InvalidToken
                });
            }

            var loggedInPatient = await _patientService.GetByUserIdAsync(userId);

            if (loggedInPatient == null)
            {
                return NotFound(new
                {
                    message = ProfileNotFound
                });
            }

            if (loggedInPatient.PatientId != id)
            {
                return StatusCode(403, new
                {
                    message = "You are not allowed to access another patient's health records"
                });
            }

            var records = await _healthRecordService.GetByPatientIdAsync(id);

            return Ok(records);
        }

        private string? GetLoggedInUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub")
                ?? User.FindFirstValue("nameid");
        }
    }
}