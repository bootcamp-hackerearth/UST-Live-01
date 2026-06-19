using HealthAxisApplicn.Dto.Doctors;
using HealthAxisApplicn.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxisApplicn.Controllers
{
    [Route("api/doctors")]
    [ApiController]
    public class DoctorController(IDoctorService service) : ControllerBase
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
        public async Task<IActionResult> Create([FromBody] CreateDoctorDto entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await service.CreateAsync(entity);
            if (result is null) return NotFound();
            return CreatedAtAction("GetById", new { id = result.DoctorId }, result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDoctorDto entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await service.UpdateAsync(id, entity);
            if (result is null) return NotFound();
            return Ok(result);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActiveDoctors()
        {
            return Ok(await service.GetActiveDoctorsAsync());
        }

        [HttpGet("name/{name}")]
        public async Task<IActionResult> SearchByName(string name)
        {
            var result = await service.SearchByNameAsync(name);
            return result.Count == 0 ? NotFound() : Ok(result);
        }

        [HttpGet("specialisation/{specialisation}")]
        public async Task<IActionResult> SearchBySpecialisation(string specialisation)
        {
            var result = await service.SearchBySpecialisationAsync(specialisation);
            return result.Count == 0 ? NotFound() : Ok(result);
        }
    }
}
