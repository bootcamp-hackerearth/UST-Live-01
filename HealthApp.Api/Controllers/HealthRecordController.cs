using HealthApp.Api.Dtos;
using HealthApp.Api.Exceptions;
using HealthApp.Api.Extensions;
using HealthApp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.HttpSys;

namespace HealthApp.Api.Controllers
{
    [ApiController]
    [Route("api/health-records")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class HealthRecordsController : ControllerBase
    {
        private readonly IHealthRecordService _healthRecordService;

        public HealthRecordsController(IHealthRecordService healthRecordService)
        {
            _healthRecordService = healthRecordService;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> GetAllHealthRecords()
        {
            var records = await _healthRecordService.GetAllAsync();

            return Ok(records);
        }

        [HttpGet("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Patient,Doctor,Admin")]
        public async Task<IActionResult> GetHealthRecordById(int id)
        {
            var record = await _healthRecordService.GetByIdAsync(id);

            if (User.IsPatient() && record.PatientId != User.GetPatientId())
            {
                throw new ForbiddenAccessException(
                    "You cannot access another patient's health record.");
            }

            if (User.IsDoctor() && record.DoctorId != User.GetDoctorId())
            {
                throw new ForbiddenAccessException(
                    "You cannot access another doctor's health record.");
            }

            return Ok(record);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Doctor")]
        public async Task<IActionResult> AddHealthRecord(
            [FromBody] HealthRecordCreateDto dto)
        {
            var loggedInDoctorId = User.GetDoctorId();

            if (loggedInDoctorId == null)
            {
                throw new ForbiddenAccessException(
                    "Doctor profile is not linked to this user.");
            }

            dto.DoctorId = loggedInDoctorId.Value;

            await _healthRecordService.AddAsync(dto);

            return StatusCode(
                StatusCodes.Status201Created,
                new { message = "Health record added successfully." });
        }

        [HttpGet("patient/{patientId:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Patient,Doctor,Admin")]
        public async Task<IActionResult> GetHealthRecordsByPatient(int patientId)
        {
            if (User.IsPatient() && User.GetPatientId() != patientId)
            {
                throw new ForbiddenAccessException(
                    "You cannot access another patient's health records.");
            }

            var records = await _healthRecordService.GetPatientHistoryAsync(patientId);

            return Ok(records);
        }
    }
}