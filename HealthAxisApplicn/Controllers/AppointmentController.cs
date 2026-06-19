using HealthAxisApplicn.Dto.Appointments;
using HealthAxisApplicn.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxisApplicn.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController(IAppointmentService service) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await service.GetByIdAsync(id);
            if (result is null) return NotFound();
            return Ok(result);
        }

        public async Task<IActionResult> Create([FromBody] AppointmentDto entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await service.CreateAsync(entity);
            if (result is null) return NotFound();
            return CreatedAtAction("GetById", new { id = result.AppointmentID }, result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] AppointmentDto entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await service.UpdatebyAsync(id, entity);
            if (result is null) return NotFound();
            return Ok(result);
        }
    }
}
