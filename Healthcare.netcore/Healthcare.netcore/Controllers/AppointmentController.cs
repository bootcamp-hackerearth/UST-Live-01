using HealthAxis.API.DTOs;
using HealthAxis.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace HealthAxis.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Patient")]
    [ApiController]
    [Route("api/appointments")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _service;

        public AppointmentController(IAppointmentService service)
        {
            _service = service;
        }

        // ✅ GET all appointments
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        // ✅ GET by id (IMPORTANT ✅)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetAllAsync();
            return Ok(result.FirstOrDefault(a => a.AppointmentId == id));
        }

        // ✅ CREATE appointment
        [HttpPost]
        public async Task<IActionResult> Create(CreateAppointmentDto dto)
        {
            var result = await _service.AddAsync(dto);
            return Ok(result);
        }

        // ✅ UPDATE STATUS
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, UpdateAppointmentStatusDto dto)
        {
            var result = await _service.UpdateStatusAsync(id, dto);
            return Ok(result);
        }

        // ✅ DELETE appointment
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            return Ok(result);
        }
    }
}