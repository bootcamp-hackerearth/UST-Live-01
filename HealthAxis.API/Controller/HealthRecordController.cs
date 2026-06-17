using HealthAxis.API.DTO;
using HealthAxis.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxis.API.Controller
{
    [Route("api/health-records")]
    [ApiController]
    //[Authorize(Roles = "Patient,Doctor,Admin")]
    public class HealthRecordController : ControllerBase
    {
        private readonly IHealthRecordService _healthRecordService;

        public HealthRecordController(
            IHealthRecordService healthRecordService)
        {
            _healthRecordService = healthRecordService;
        }

        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetByPatientId(int patientId)
        {
            var records =
                await _healthRecordService.GetByPatientIdAsync(patientId);

            return Ok(records);
        }

        [HttpPost]
        //[Authorize(Roles = "Doctor,Admin")]
        public async Task<IActionResult> CreateHealthRecord(
            [FromBody] CreateHealthRecordDto healthRecordDto)
        {
            var record =
                await _healthRecordService.AddAsync(healthRecordDto);

            return Ok(record);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var record =
                await _healthRecordService.GetByIdAsync(id);

            return Ok(record);
        }
    }
}