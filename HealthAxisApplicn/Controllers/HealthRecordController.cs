using HealthAxisApplicn.Dto.HealthRecords;
using HealthAxisApplicn.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxisApplicn.Controllers
{
    [Route("api/healthrecords")]
    [ApiController]
    public class HealthRecordController(IHealthRecordService service) : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await service.GetByIdAsync(id);
            if (result is null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateHealthRecordDto entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await service.CreateAsync(entity);
            if (result is null) return NotFound();
            return CreatedAtAction("GetById", new { id = result.HealthRecordId }, result);
        }

        [HttpGet("patient/id/{id:int}")]
        public async Task<IActionResult> GetByPatientId([FromRoute] int id)
        {
            var result = await service.GetRecordsByPatientIdAsync(id);
            return result.Count == 0 ? NotFound() : Ok(result);
        }

        [HttpGet("doctor/id/{id:int}")]
        public async Task<IActionResult> GetByDoctorId([FromRoute] int id)
        {
            var result = await service.GetRecordsByDoctorIdAsync(id);
            return result.Count == 0 ? NotFound() : Ok(result);
        }

        [HttpGet("patient/name/{name}")]
        public async Task<IActionResult> GetByPatientName([FromRoute] string name)
        {
            var result = await service.GetRecordsByPatientNameAsync(name);
            return result.Count == 0 ? NotFound() : Ok(result);
        }

        [HttpGet("doctor/name/{name}")]
        public async Task<IActionResult> GetByDoctorName([FromRoute] string name)
        {
            var result = await service.GetRecordsByDoctorNameAsync(name);
            return result.Count == 0 ? NotFound() : Ok(result);
        }
    }
}
