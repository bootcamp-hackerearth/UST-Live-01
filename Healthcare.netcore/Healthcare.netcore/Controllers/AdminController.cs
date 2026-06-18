using HealthAxis.API.DTOs;
using HealthAxis.API.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxis.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    [ApiController]
    [Route("api/admin")]
    public class AdminController : ControllerBase
    {
        private readonly IDoctorService _doctorService;
        private readonly IAppointmentService _appointmentService;

        public AdminController(
            IDoctorService doctorService,
            IAppointmentService appointmentService)
        {
            _doctorService = doctorService;
            _appointmentService = appointmentService;
        }

        [HttpGet("doctors")]
        public async Task<IActionResult> GetDoctors()
        {
            var result = await _doctorService.GetAllAsync();
            return Ok(result);
        }

        [HttpPost("doctors")]
        public async Task<IActionResult> CreateDoctor(CreateDoctorDto dto)
        {
            var result = await _doctorService.AddAsync(dto);
            return Ok(result);
        }

        [HttpPut("doctors/{id}")]
        public async Task<IActionResult> UpdateDoctor(int id, UpdateDoctorDto dto)
        {
            var result = await _doctorService.UpdateAsync(id, dto);
            return Ok(result);
        }

        [HttpGet("reports/appointments")]
        public async Task<IActionResult> GetReports()
        {
            var result = await _appointmentService.GetAllAsync();
            return Ok(result);
        }
    }
}