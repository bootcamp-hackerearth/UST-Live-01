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

        // Patient/Doctor: view logged-in user's health records
        [HttpGet("my")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Patient,Doctor")]
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

            if (User.IsInRole("Patient"))
            {
                var records = await service.GetMyHealthRecordsForPatientAsync(identityUserId);

                return Ok(records);
            }

            if (User.IsInRole("Doctor"))
            {
                var records = await service.GetMyHealthRecordsForDoctorAsync(identityUserId);

                return Ok(records);
            }

            return Forbid();
        }

        // Admin, Doctor, Patient: view health record by id
        // Patient and Doctor ownership are checked here.
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

            var identityUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(identityUserId))
            {
                return Unauthorized(new
                {
                    Message = "Invalid user token."
                });
            }

            if (User.IsInRole("Patient"))
            {
                var record = await service.GetHealthRecordByIdForPatientAsync(
                    healthRecordId,
                    identityUserId);

                return Ok(record);
            }

            if (User.IsInRole("Doctor"))
            {
                var record = await service.GetHealthRecordByIdForDoctorAsync(
                    healthRecordId,
                    identityUserId);

                return Ok(record);
            }

            return Forbid();
        }

        // Admin only: view records by patient id
        // Patient should use GET /api/HealthRecords/my
        // Doctor should also not use random patientId route.
        [HttpGet("patient/{patientId:int}")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin")]
        public async Task<IActionResult> GetHealthRecordsByPatientId([FromRoute] int patientId)
        {
            var records = await service.GetHealthRecordsByPatientIdAsync(patientId);

            return Ok(records);
        }

        // Admin only: view records by doctor id
        // Doctor should use GET /api/HealthRecords/my
        [HttpGet("doctor/{doctorId:int}")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin")]
        public async Task<IActionResult> GetHealthRecordsByDoctorId([FromRoute] int doctorId)
        {
            var records = await service.GetHealthRecordsByDoctorIdAsync(doctorId);

            return Ok(records);
        }

        // Admin/Doctor: view records by appointment
        // Doctor ownership is checked here.
        [HttpGet("appointment/{appointmentId:int}")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin,Doctor")]
        public async Task<IActionResult> GetHealthRecordsByAppointmentId([FromRoute] int appointmentId)
        {
            if (User.IsInRole("Admin"))
            {
                var records = await service.GetHealthRecordsByAppointmentIdAsync(appointmentId);

                return Ok(records);
            }

            var identityUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(identityUserId))
            {
                return Unauthorized(new
                {
                    Message = "Invalid user token."
                });
            }

            var doctorRecords = await service.GetHealthRecordsByAppointmentIdForDoctorAsync(
                appointmentId,
                identityUserId);

            return Ok(doctorRecords);
        }

        // Doctor only: add health record
        // Doctor ownership is checked inside service.
        [HttpPost]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Doctor")]
        public async Task<IActionResult> AddHealthRecord([FromBody] AddHealthRecordDto request)
        {
            var identityUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(identityUserId))
            {
                return Unauthorized(new
                {
                    Message = "Invalid user token."
                });
            }

            var record = await service.AddHealthRecordForDoctorAsync(
                request,
                identityUserId);

            return CreatedAtAction(
                nameof(GetHealthRecordById),
                new { healthRecordId = record.HealthRecordId },
                record);
        }

        // Doctor only: update health record
        // Doctor ownership is checked inside service.
        [HttpPut("{healthRecordId:int}")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Doctor")]
        public async Task<IActionResult> UpdateHealthRecord(
            [FromRoute] int healthRecordId,
            [FromBody] UpdateHealthRecordDto request)
        {
            var identityUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(identityUserId))
            {
                return Unauthorized(new
                {
                    Message = "Invalid user token."
                });
            }

            var record = await service.UpdateHealthRecordForDoctorAsync(
                healthRecordId,
                request,
                identityUserId);

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