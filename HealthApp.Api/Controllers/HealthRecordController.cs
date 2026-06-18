using HealthApp.Api.Dtos;
using HealthApp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HealthApp.Api.Controllers
{
    [ApiController]
    [Route("api/health-records")]
    public class HealthRecordsController : ControllerBase
    {
        private readonly IHealthRecordService _healthRecordService;

        public HealthRecordsController(IHealthRecordService healthRecordService)
        {
            _healthRecordService = healthRecordService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllHealthRecords()
        {
            var records = await _healthRecordService.GetAllAsync();

            return Ok(records);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetHealthRecordById(int id)
        {
            var record = await _healthRecordService.GetByIdAsync(id);

            return Ok(record);
        }

        [HttpPost]
        public async Task<IActionResult> AddHealthRecord(
            [FromBody] HealthRecordCreateDto dto)
        {
            await _healthRecordService.AddAsync(dto);

            return StatusCode(
                StatusCodes.Status201Created,
                new { message = "Health record added successfully." });
        }

        [HttpGet("patient/{patientId:int}")]
        public async Task<IActionResult> GetHealthRecordsByPatient(int patientId)
        {
            var records = await _healthRecordService.GetPatientHistoryAsync(patientId);

            return Ok(records);
        }
    }
}