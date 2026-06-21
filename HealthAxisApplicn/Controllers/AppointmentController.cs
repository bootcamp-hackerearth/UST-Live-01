using HealthAxisApplicn.Dto.Appointments;
using HealthAxisApplicn.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxisApplicn.Controllers
{
    [Route("api/appointments")]
    [ApiController]
    [Authorize]
    public class AppointmentController(IAppointmentService service) : ControllerBase
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Patient")]
        public async Task<IActionResult> Create([FromBody] CreateAppointmentDto entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await service.CreateAsync(entity);
            if (result is null) return NotFound();
            return CreatedAtAction("GetById", new { id = result.AppointmentId }, result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAppointmentStatusDto entity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await service.UpdateAsync(id, entity);
            if (result is null) return NotFound();
            return Ok(result);
        }

        [HttpGet("patient/{patientId:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> GetByPatient(int patientId)
        {
            var result = await service.GetAppointmentsByPatientIdAsync(patientId);
            return result.Count == 0 ? NotFound() : Ok(result);
        }

        [HttpGet("doctor/{doctorId:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> GetByDoctor(int doctorId)
        {
            var result = await service.GetAppointmentsByDoctorIdAsync(doctorId);
            return result.Count == 0 ? NotFound() : Ok(result);
        }
        [HttpDelete("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        { 
            await service.DeleteAppointmentAsync(id); 
            return NoContent(); 
        }
    }
}
