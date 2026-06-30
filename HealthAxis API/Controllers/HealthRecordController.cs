using HealthAxis.API.DTOs.HealthRecords;
using HealthAxis.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxis.API.Controllers
{
    [Route("api/health-records")]
    [ApiController]
    [Authorize(Roles = "Patient,Doctor,Admin")]
    public class HealthRecordsController : ControllerBase
    {
        private readonly IHealthRecordService _healthRecordService;

        public HealthRecordsController(
            IHealthRecordService healthRecordService)
        {
            _healthRecordService = healthRecordService;
        }

        [HttpGet("{patientId:int}")]
        public async Task<IActionResult> GetByPatientId(
            int patientId,
            CancellationToken ct)
        {
            var records =
                await _healthRecordService.GetByPatientIdAsync(
                    patientId,
                    ct);

            return Ok(records);
        }

        [HttpPost]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<IActionResult> Create(
            HealthRecordCreateDto request,
            CancellationToken ct)
        {
            var record =
                await _healthRecordService.CreateAsync(
                    request,
                    ct);

            return Ok(record);
        }

        [HttpGet("details/{id:int}")]
        public async Task<IActionResult> GetById(
            int id,
            CancellationToken ct)
        {
            var record =
                await _healthRecordService.GetByIdAsync(
                    id,
                    ct);

            if (record == null)
            {
                return NotFound(new
                {
                    message = "Health record not found."
                });
            }

            return Ok(record);
        }
    }
}
