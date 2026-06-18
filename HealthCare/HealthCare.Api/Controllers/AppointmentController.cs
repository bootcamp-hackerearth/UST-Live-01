using HealthCare.Api.DTOs.Appointment;
using HealthCare.Api.DTOs.Appointments;
using HealthCare.Api.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthCare.Api.Controllers
{
    
    [Route("api/appointments")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _service;

        public AppointmentController(IAppointmentService service)
        {
            _service = service;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll(AppointmentFilter filter)
        {
            var appointments = await _service.GetAllAsync( filter);
            return Ok(appointments);
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var appointment = await _service.GetByIdAsync(id);

            if (appointment is null)
                return NotFound();

            return Ok(appointment);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAppointmentDto dto,int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _service.AddAsync(dto,id);

            return StatusCode(201, new
            {
                message = "Appointment created successfully"
            });
        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAppointmentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _service.UpdateAsync(id, dto);

            return NoContent();
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return NoContent();
        }

    }
}
