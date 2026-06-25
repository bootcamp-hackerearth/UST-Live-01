using Healthcare.Shared.DTOs.Appointments;
using HealthCare.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;

namespace HealthCare.Api.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminAppointmentController : ControllerBase
    {

        private readonly IAppointmentService _appointmentService;
        public AdminAppointmentController(IAppointmentService appointmentService)
        {

            _appointmentService = appointmentService;
        }

        [HttpGet("/appointments")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> GetAllAppointment([FromQuery] AppointmentFilter filter)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _appointmentService.GetAllAsync(filter);
            return Ok(result);
        }

        [HttpGet("/appointments/report")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetDailyReport()
        {
            var result = await _appointmentService.GetDailyReport();
            return Ok(result);
        }


        [HttpGet("dashboard")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> GetDashboard()
        {
            var result = await _appointmentService.GetSummaryAsync();
            return Ok(result);
        }

    }
}
