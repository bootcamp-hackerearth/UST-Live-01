using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using S3_HealthAxisApi.DTOs.HealthRecord;
using S3_HealthAxisApi.Services.Interface;

namespace S3_HealthAxisApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HealthRecordsController : ControllerBase
    {
        private readonly IHealthRecordService _healthRecordService;

        public HealthRecordsController(
            IHealthRecordService healthRecordService)
        {
            _healthRecordService = healthRecordService;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var record =
                await _healthRecordService.GetByIdAsync(id);

            if (record == null)
            {
                return NotFound(
                    $"Health record {id} not found.");
            }

            return Ok(record);
        }

        [HttpGet("appointment/{appointmentId:int}")]
        public async Task<IActionResult> GetByAppointment(
            int appointmentId)
        {
            var record =
                await _healthRecordService
                    .GetByAppointmentIdAsync(appointmentId);

            if (record == null)
            {
                return NotFound(
                    "Health record not found for this appointment.");
            }

            return Ok(record);
        }

        [HttpPost]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<IActionResult> Create(
            [FromBody] CreateHealthRecordDto dto)
        {
            try
            {
                var record =
                    await _healthRecordService.CreateAsync(dto);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = record.HealthRecordId },
                    record);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateHealthRecordDto dto)
        {
            try
            {
                await _healthRecordService
                    .UpdateAsync(id, dto);

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}