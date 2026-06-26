using HealthAxis.Shared.DTOs.Appointment;
using HealthAxis.Shared.Enums;
using HealthAxisCore_Api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxisCore_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _service;

        public AppointmentController(IAppointmentService service)
        {
            _service = service;
        }

        
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }
        [HttpGet("paged")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> GetPaged(
    int pageNumber = 1,
    int pageSize = 10,
    string? search = null,
    AppointmentStatus? status = null,
    DateTime? startDate = null,
    DateTime? endDate = null)
        {
            var result = await _service.GetPagedAsync(
                pageNumber,
                pageSize,
                search,
                status,
                startDate,
                endDate
            );

            return Ok(result);
        }


        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null) return NotFound();

            return Ok(result);
        }

        
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Patient")]
        public async Task<IActionResult> Create(CreateAppointmentDTO dto)
        {
            var result = await _service.CreateAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = result.AppointmentId }, result);
        }

        
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();

            return Ok("Deleted successfully");
        }

        
        [HttpGet("doctor/{doctorId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Doctor,Admin")]
        public async Task<IActionResult> GetByDoctor(int doctorId)
        {
            return Ok(await _service.GetByDoctorAsync(doctorId));
        }

        
        [HttpGet("patient/{patientId}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Patient,Admin")]
        public async Task<IActionResult> GetByPatient(int patientId)
        {
            return Ok(await _service.GetByPatientAsync(patientId));
        }

        
        [HttpGet("filter")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Doctor")]
        public async Task<IActionResult> Filter(AppointmentStatus? status, DateTime? startDate, DateTime? endDate)
        {
            return Ok(await _service.FilterAsync(status, startDate, endDate));
        }

        
        [HttpPost("confirm/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Doctor,Admin")]
        public async Task<IActionResult> Confirm(int id)
        {
            var result = await _service.ConfirmAsync(id);
            if (!result) return NotFound();

            return Ok("Confirmed");
        }

       
        [HttpPost("cancel/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Cancel(int id, string reason)
        {
            var result = await _service.CancelAsync(id, reason);
            if (!result) return NotFound();

            return Ok("Cancelled");
        }
    }
}