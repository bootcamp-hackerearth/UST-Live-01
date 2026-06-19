using HealthCare.Api.DTOs.Doctor;
using HealthCare.Api.DTOs.Patient;
using HealthCare.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthCare.Api.Controllers
{
    [Route("api/doctors")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _service;

        public DoctorController(IDoctorService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll(DoctorFilter filter)
        {
            var doctors = await _service.GetAllAsync(filter);
            return Ok(doctors);
        }

        [HttpGet("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(int id)
        {
            var doctor = await _service.GetByIdAsync(id);

            if (doctor is null)
                return NotFound();

            return Ok(doctor);
        }


        [HttpPut("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDoctorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _service.UpdateAsync(id, dto);

            return NoContent();
        }


        [HttpDelete("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);

            return NoContent();
        }


        [HttpPatch("{id:int}/status")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStatus(int id, [FromQuery] bool isActive)
        {
            await _service.UpdateStatusAsync(id, isActive);
            return NoContent();
        }


        [HttpPost("{id:int}/slots")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> CreateSlots(int id, [FromBody] List<string> timeSlots)
        {
            if (timeSlots == null || !timeSlots.Any())
                return BadRequest("Time slots are required");

            await _service.CreateSlots(id, timeSlots);

            return Ok("Slots created successfully");
        }


        [HttpGet("{id:int}/slots")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> GetSlots(int id)
        {
            var slots = await _service.GetSlots(id);
            return Ok(slots);
        }


        [HttpGet("{id:int}/available-slots")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Patient,Admin")]
        public async Task<IActionResult> AvailableSlots(int id, [FromQuery] DateOnly date)
        {
            var result = await _service.AvailableTimeSlotsCheck(date, id);
            return Ok(result);
        }


        [HttpPost("{id:int}/leaves")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateLeave(int id, [FromBody] List<CreateLeaveDto> leaves)
        {
            if (leaves == null || !leaves.Any())
                return BadRequest("Leave list is required");

            var result = await _service.CreateLeave(id, leaves);

            return Ok(result);
        }


        [HttpGet("available")]
        [AllowAnonymous]
        public async Task<IActionResult> AvailableDoctors(
                    [FromQuery] string specialisation,
                    [FromQuery] DateOnly date)
        {
            if (string.IsNullOrWhiteSpace(specialisation))
                return BadRequest("Specialisation is required");

            var result = await _service.AvailableDoctors(specialisation, date);

            return Ok(result);
        }



    }
}
