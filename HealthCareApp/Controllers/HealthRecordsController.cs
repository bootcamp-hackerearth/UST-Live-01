using HealthCareApp.Dtos;
using HealthCareApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedClasses.Dtos;

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

        // Admin, Doctor, Patient: view health record by id
        // Later ownership check:
        // Patient should view only own record.
        // Doctor should view only related record.
        [HttpGet("{healthRecordId:int}")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin,Doctor,Patient")]
        public async Task<IActionResult> GetHealthRecordById([FromRoute] int healthRecordId)
        {
            var record = await service.GetHealthRecordByIdAsync(healthRecordId);

            return Ok(record);
        }

        // Admin, Patient, Doctor: view records by patient
        // Later:
        // Patient should access only own patientId.
        // Doctor should access only patients linked to their appointments.
        [HttpGet("patient/{patientId:int}")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin,Patient,Doctor")]
        public async Task<IActionResult> GetHealthRecordsByPatientId([FromRoute] int patientId)
        {
            var records = await service.GetHealthRecordsByPatientIdAsync(patientId);

            return Ok(records);
        }

        // Admin, Doctor: view records by doctor
        // Later:
        // Doctor should access only own doctorId.
        [HttpGet("doctor/{doctorId:int}")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin,Doctor")]
        public async Task<IActionResult> GetHealthRecordsByDoctorId([FromRoute] int doctorId)
        {
            var records = await service.GetHealthRecordsByDoctorIdAsync(doctorId);

            return Ok(records);
        }

        // Admin, Doctor: view records by appointment
        // Later:
        // Doctor should access only appointment assigned to them.
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