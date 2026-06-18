using HealthCareApp.Dtos;
using HealthCareApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        // Admin only.
        // Not in company requirement, but useful for admin portal.
        [HttpGet]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin")]
        public async Task<IActionResult> GetAllPatients()
        {
            var patients = await _patientService.GetAllPatientsAsync();

            return Ok(patients);
        }

        // Admin and Patient can access patient profile.
        // Later we should add ownership check so Patient can access only own profile.
        [HttpGet("{patientId:int}")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin,Patient")]
        public async Task<IActionResult> GetPatientById([FromRoute] int patientId)
        {
            var patient = await _patientService.GetPatientByIdAsync(patientId);

            return Ok(patient);
        }

        // Admin and Patient can update patient profile.
        // Later we should add ownership check so Patient can update only own profile.
        [HttpPut("{patientId:int}")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin,Patient")]
        public async Task<IActionResult> UpdatePatient(
            [FromRoute] int patientId,
            [FromBody] UpdatePatientDto dto)
        {
            var patient = await _patientService.UpdatePatientAsync(patientId, dto);

            return Ok(patient);
        }

        // Admin, Patient, and Doctor can view patient health records.
        // Later:
        // Patient should see only own records.
        // Doctor should see only records related to assigned appointments/patients.
        [HttpGet("{patientId:int}/health-records")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin,Patient,Doctor")]
        public async Task<IActionResult> GetPatientHealthRecords([FromRoute] int patientId)
        {
            var records = await _healthRecordService.GetHealthRecordsByPatientIdAsync(patientId);

            return Ok(records);
        }
    }
}