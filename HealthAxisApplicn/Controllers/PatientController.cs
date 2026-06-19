using HealthAxisApplicn.Dto.Patients;
using HealthAxisApplicn.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxisApplicn.Controllers
{
    [Route("api/patients")]
    [ApiController]
    public class PatientController(IPatientService service) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await service.GetByIdAsync(id);
            if (result is null) return NotFound();
            return Ok(result);
        }

        public async Task<IActionResult> Create([FromBody] PatientDto entity)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await service.CreateAsync(entity);
            if (result is null) return NotFound();
            return CreatedAtAction("GetById", new {id = result.PatientId}, result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] PatientDto entity)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await service.UpdatebyAsync(id, entity);
            if (result is null) return NotFound();
            return Ok(result);
        }

        [HttpPut("deactivate/{id:int}")]
        public async Task<IActionResult> Deactivate(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await service.DeactivatePatientAsync(id);
            if (result is null) return NotFound();
            return Ok(result);
        }

        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetByPatientName(string name)
        {
            var result = await service.SearchByPatientNameAsync(name);
            if (result is null) return NotFound();
            return Ok(result);
        }


    }
}
