using Healthcare.Shared.DTOs.Patient;
using HealthCare.Api.Services.Implementations;
using HealthCare.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthCare.Api.Controllers
{
    [Route("api/patients")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _service;
        private readonly IHealthRecordService _healthRecordService;

        public PatientController(IPatientService service,IHealthRecordService healthRecordService)
        {
            _service = service;
            _healthRecordService = healthRecordService;
        }

        //Get Profile

        [HttpGet("Profile")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Patient")]
        public async Task<IActionResult> GetMyProfile()
        {
            var patientId = GetPatientIdFromClaims();

            var patient = await _service.GetByIdAsync(patientId);

            return Ok(patient);
        }

        [HttpPut("Profile")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Patient")]
        public async Task<IActionResult> Update( UpdatePatientDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var patientId = GetPatientIdFromClaims();
            await _service.UpdateAsync(patientId, dto);
            return Ok(new { message = "Patient profile updated successfully" });
        }

        [HttpGet("my-records")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Patient")]
        public async Task<IActionResult> GetMyRecords()
        {
            var patientId = GetPatientIdFromClaims();

            var result = await _healthRecordService
                .GetHealthRecordByPatient(patientId);

            return Ok(result);
        }



        [HttpGet("dashboard/stats")]
        public IActionResult GetStats()
        {
            return Ok(new
            {
                upcoming = 2,
                completed = 10,
                records = 5,
                prescriptions = 3
            });
        }

        private int GetPatientIdFromClaims()
        {
            var claim = User.FindFirst("PatientId")
                ?? throw new InvalidOperationException("PatientId claim not found in token.");

            return int.Parse(claim.Value);
        }


    }
}
