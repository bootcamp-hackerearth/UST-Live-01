using HealthAxisCore_Api.DTOs.HealthRecord;
using HealthAxisCore_Api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxisCore_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthRecordController : ControllerBase
    {
        private readonly IHealthRecordService _service;

        public HealthRecordController(IHealthRecordService service)
        {
            _service = service;
        }

       
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Doctor,Admin")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);

            if (result == null)
                return NotFound("Health record not found");

            return Ok(result);
        }

        
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Doctor")]
        public async Task<IActionResult> Create(CreateHealthRecordDTO dto)
        {
            var result = await _service.CreateAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = result.HealthRecordId }, result);
        }

       
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);

            if (!deleted)
                return NotFound("Health record not found");

            return Ok("Health record deleted successfully");
        }

        
        [HttpGet("patient/{patientId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Doctor,Admin,Patient")]
        public async Task<IActionResult> GetByPatient(int patientId)
        {
            return Ok(await _service.GetByPatientAsync(patientId));
        }
    }
}