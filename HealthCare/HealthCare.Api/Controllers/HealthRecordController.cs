using HealthCare.Api.DTOs.HealthRecord;
using HealthCare.Api.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthCare.Api.Controllers
{
    [Route("api/healthrecords")]
    [ApiController]
    public class HealthRecordController : ControllerBase
    {

        private readonly IHealthRecordService _healthRecordService;

        public HealthRecordController(IHealthRecordService healthRecordService)
        {
            _healthRecordService = healthRecordService;
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateHealthRecordDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _healthRecordService.AddAsync(dto);

            return Ok(new { message = "Health record created successfully" });
        }

        // ✅ GET ALL Health Records
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var records = await _healthRecordService.GetAllAsync();
            return Ok(records);
        }

        // ✅ GET Health Record By ID
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var record = await _healthRecordService.GetByIdAsync(id);

            if (record == null)
                return NotFound();

            return Ok(record);
        }

        // ✅ UPDATE Health Record
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateHealthRecordDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _healthRecordService.UpdateAsync(id, dto);

            return Ok(new { message = "Health record updated successfully" });
        }

        // ✅ DELETE Health Record
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _healthRecordService.DeleteAsync(id);

            return NoContent(); // 204
        }


    }
}
