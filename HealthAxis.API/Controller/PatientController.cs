using HealthAxis.Shared.DTO.PatientDtos;
using HealthAxis.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        private string? GetLoggedInUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }


        [HttpGet("me")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = GetLoggedInUserId();

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized(new
                {
                    message = "Invalid token"
                });
            }

            var patient = await _patientService.GetByUserIdAsync(userId);

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
                    message = "Invalid token"
                });
            }

            var loggedInPatient = await _patientService.GetByUserIdAsync(userId);

            if (loggedInPatient == null)
            {
                return NotFound(new
                {
                    message = "Patient profile not found"
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
                    message = "Invalid token"
                });
            }

            var loggedInPatient = await _patientService.GetByUserIdAsync(userId);

            if (loggedInPatient == null)
            {
                return NotFound(new
                {
                    message = "Patient profile not found"
                });
            }

            var records = await _patientService.GetHealthRecordsByPatientIdAsync(
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
                    message = "Invalid token"
                });
            }

            var loggedInPatient = await _patientService.GetByUserIdAsync(userId);

            if (loggedInPatient == null)
            {
                return NotFound(new
                {
                    message = "Patient profile not found"
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
                    message = "Invalid token"
                });
            }

            var loggedInPatient = await _patientService.GetByUserIdAsync(userId);

            if (loggedInPatient == null)
            {
                return NotFound(new
                {
                    message = "Patient profile not found"
                });
            }

            if (loggedInPatient.PatientId != id)
            {
                return StatusCode(403, new
                {
                    message = "You cannot update another patient's details"
                });
            }

            var updatedPatient = await _patientService.UpdateAsync(id, patientDto);

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
                    message = "Invalid token"
                });
            }

            var loggedInPatient = await _patientService.GetByUserIdAsync(userId);

            if (loggedInPatient == null)
            {
                return NotFound(new
                {
                    message = "Patient profile not found"
                });
            }

            if (loggedInPatient.PatientId != id)
            {
                return StatusCode(403, new
                {
                    message = "You are not allowed to access another patient's health records"
                });
            }

            var records = await _patientService.GetHealthRecordsByPatientIdAsync(id);

            return Ok(records);
        }
    }
}