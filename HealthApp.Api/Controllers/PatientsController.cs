using HealthApp.Api.Dtos;
using HealthApp.Api.Exceptions;
using HealthApp.Api.Extensions;
using HealthApp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthApp.Api.Controllers
{
    [ApiController]
    [Route("api/patients")]
    [Authorize]
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

        [HttpGet("profile")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Patient")]
        public async Task<IActionResult> GetMyProfile()
        {
            var patientId = User.GetPatientId();

            if (patientId == null)
            {
                throw new ForbiddenAccessException(
                    "Patient profile is not linked to this user.");
            }

            var patient = await _patientService.GetPatientByIdAsync(patientId.Value);

            return Ok(patient);
        }

        [HttpPut("profile")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Patient")]
        public async Task<IActionResult> UpdateMyProfile([FromBody] PatientCreateDto dto)
        {
            var patientId = User.GetPatientId();

            if (patientId == null)
            {
                throw new ForbiddenAccessException(
                    "Patient profile is not linked to this user.");
            }

            await _patientService.UpdatePatientAsync(patientId.Value, dto);

            return Ok(new
            {
                message = "Patient profile updated successfully."
            });
        }

        [HttpGet("profile/health-records")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Patient")]
        public async Task<IActionResult> GetMyHealthRecords()
        {
            var patientId = User.GetPatientId();

            if (patientId == null)
            {
                throw new ForbiddenAccessException(
                    "Patient profile is not linked to this user.");
            }

            var records = await _healthRecordService.GetPatientHistoryAsync(patientId.Value);

            return Ok(records);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> GetAllPatients()
        {
            var patients = await _patientService.GetAllPatientsAsync();

            return Ok(patients);
        }

        [HttpGet("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Doctor,Admin")]
        public async Task<IActionResult> GetPatientById(int id)
        {
            var patient = await _patientService.GetPatientByIdAsync(id);

            return Ok(patient);
        }

        [HttpPut("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> UpdatePatient(
            int id,
            [FromBody] PatientCreateDto dto)
        {
            await _patientService.UpdatePatientAsync(id, dto);

            return Ok(new
            {
                message = "Patient updated successfully."
            });
        }

        [HttpGet("{id:int}/health-records")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Doctor,Admin")]
        public async Task<IActionResult> GetPatientHealthRecords(int id)
        {
            var records = await _healthRecordService.GetPatientHistoryAsync(id);

            return Ok(records);
        }

        [HttpGet("search")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Doctor,Admin")]
        public async Task<IActionResult> SearchPatients(
            [FromQuery] string? name,
            [FromQuery] string? email)
        {
            var patients = await _patientService.SearchPatientsAsync(name, email);

            return Ok(patients);
        }
    }
}