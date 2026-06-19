using HealthAxisApplicn.Dto.HealthRecords;
using HealthAxisApplicn.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxisApplicn.Controllers
{
    [Route("api/healthrecord")]
    [ApiController]
    public class HealthRecordController(IHealthRecordService service) : ControllerBase
    {
        

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await service.GetByIdAsync(id);
            if (result is null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] HealthRecordDto entity)
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
            var result = await service.GetRecordByPatientIDAsync(id);
            if (result is null) return NotFound();
            return Ok(result);
        }

        [HttpGet("doctor/id/{id:int}")]
        public async Task<IActionResult> GetByDoctorId([FromRoute] int id)
        {
            var result = await service.GetRecordsByDoctorIDAsync(id);
            if (result is null) return NotFound();
            return Ok(result);
        }

        [HttpGet("patient/name/{name}")]
        public async Task<IActionResult> GetByPatientName([FromRoute] string name)
        {
            var result = await service.GetRecordsByPatientNameAsync(name);
            if (result is null) return NotFound();
            return Ok(result);
        }

        [HttpGet("doctor/name/{name}")]
        public async Task<IActionResult> GetByDoctorName([FromRoute] string name)
        {
            var result = await service.GetRecordsByDoctorNameAsync(name);
            if (result is null) return NotFound();
            return Ok(result);
        }
    }
}
