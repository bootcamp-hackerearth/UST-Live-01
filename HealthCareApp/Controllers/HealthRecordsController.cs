using HealthCareApp.Dtos;
using HealthCareApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedClasses.Dtos;
using System.Security.Claims;

namespace HealthCareApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthRecordsController(IHealthRecordService service) : ControllerBase
    {
        // Admin only: view all health records
        [HttpGet]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin")]
        public async Task<IActionResult> GetAllHealthRecords()
        {
            var records = await service.GetAllHealthRecordsAsync();

            return Ok(records);
        }

        // Patient only: view logged-in patient's health records
        [HttpGet("my")]
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

            var records = await service.GetMyHealthRecordsForPatientAsync(identityUserId);

            return Ok(records);
        }

        // Admin, Doctor, Patient: view health record by id
        // Patient ownership is checked here.
        // Doctor ownership will be handled later.
        [HttpGet("{healthRecordId:int}")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin,Doctor,Patient")]
        public async Task<IActionResult> GetHealthRecordById([FromRoute] int healthRecordId)
        {
            if (User.IsInRole("Admin"))
            {
                var record = await service.GetHealthRecordByIdAsync(healthRecordId);

                return Ok(record);
            }

            if (User.IsInRole("Patient"))
            {
                var identityUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrWhiteSpace(identityUserId))
                {
                    return Unauthorized(new
                    {
                        Message = "Invalid user token."
                    });
                }

                var record = await service.GetHealthRecordByIdForPatientAsync(
                    healthRecordId,
                    identityUserId);

                return Ok(record);
            }

            if (User.IsInRole("Doctor"))
            {
                // Doctor ownership will be handled later.
                var record = await service.GetHealthRecordByIdAsync(healthRecordId);

                return Ok(record);
            }

            return Forbid();
        }

        // Admin and Doctor: view records by patient id
        // Patient should use GET /api/HealthRecords/my
        // Doctor ownership will be handled later.
        [HttpGet("patient/{patientId:int}")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin,Doctor")]
        public async Task<IActionResult> GetHealthRecordsByPatientId([FromRoute] int patientId)
        {
            var records = await service.GetHealthRecordsByPatientIdAsync(patientId);

            return Ok(records);
        }

        // Admin and Doctor: view records by doctor id
        // Doctor ownership will be handled later.
        [HttpGet("doctor/{doctorId:int}")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin,Doctor")]
        public async Task<IActionResult> GetHealthRecordsByDoctorId([FromRoute] int doctorId)
        {
            var records = await service.GetHealthRecordsByDoctorIdAsync(doctorId);

            return Ok(records);
        }

        // Admin and Doctor: view records by appointment
        // Doctor ownership will be handled later.
        [HttpGet("appointment/{appointmentId:int}")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin,Doctor")]
        public async Task<IActionResult> GetHealthRecordsByAppointmentId([FromRoute] int appointmentId)
        {
            var records = await service.GetHealthRecordsByAppointmentIdAsync(appointmentId);

            return Ok(records);
        }

        // Doctor only: add health record
        // Doctor ownership will be handled later.
        [HttpPost]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Doctor")]
        public async Task<IActionResult> AddHealthRecord([FromBody] AddHealthRecordDto request)
        {
            var record = await service.AddHealthRecordAsync(request);

            return CreatedAtAction(
                nameof(GetHealthRecordById),
                new { healthRecordId = record.HealthRecordId },
                record);
        }

        // Doctor only: update health record
        // Doctor ownership will be handled later.
        [HttpPut("{healthRecordId:int}")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Doctor")]
        public async Task<IActionResult> UpdateHealthRecord(
            [FromRoute] int healthRecordId,
            [FromBody] UpdateHealthRecordDto request)
        {
            var record = await service.UpdateHealthRecordAsync(healthRecordId, request);

            return Ok(record);
        }

        // Admin only: delete health record
        [HttpDelete("{healthRecordId:int}")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin")]
        public async Task<IActionResult> DeleteHealthRecord([FromRoute] int healthRecordId)
        {
            var record = await service.DeleteHealthRecordAsync(healthRecordId);

            return Ok(record);
        }
    }
}