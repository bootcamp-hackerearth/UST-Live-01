using HealthAxis.API.DTO.HealthRecordDtos;
using HealthAxis.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxis.API.Controller
{
    [Route("api/health-records")]
    [ApiController]
    public class HealthRecordController : ControllerBase
    {
        private readonly IHealthRecordService _healthRecordService;

        public HealthRecordController(
            IHealthRecordService healthRecordService)
        {
            _healthRecordService = healthRecordService;
        }

        [HttpGet("patient/{patientId}")]
        [Authorize(Roles = "Patient,Doctor")]
        public async Task<IActionResult> GetByPatientId(int patientId)
        {
            var records =
                await _healthRecordService.GetByPatientIdAsync(patientId);

            return Ok(records);
        }

        [HttpPost]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> CreateHealthRecord(
            [FromBody] CreateHealthRecordDto healthRecordDto)
        {
            var record =
                await _healthRecordService.AddAsync(healthRecordDto);

            return Ok(record);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Patient,Doctor")]
        public async Task<IActionResult> GetById(int id)
        {
            var record =
                await _healthRecordService.GetByIdAsync(id);

            return Ok(record);
        }
    }
}