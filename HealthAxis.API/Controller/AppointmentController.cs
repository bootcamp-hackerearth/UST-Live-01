using HealthAxis.API.DTO;
using HealthAxis.API.Services.Interfaces;
using HealthAxis.DTO.AppointmentDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxis.API.Controllers
{
    [Route("api/appointments")]
    [ApiController]
    //[Authorize(Roles = "Patient,Doctor,Admin")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentsController(
            IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAppointments()
        {
            var appointments =
                await _appointmentService.GetAllAsync();

            return Ok(appointments);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAppointment(
            [FromBody] CreateAppointmentDto appointmentDto)
        {
            var appointment =
                await _appointmentService.AddAsync(appointmentDto);

            return Ok(appointment);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateAppointmentStatus(
            int id,
            [FromBody] UpdateAppointmentStatusDto statusDto)
        {
            var appointment =
                await _appointmentService.UpdateStatusAsync(
                    id,
                    statusDto);

            return Ok(appointment);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var appointment =
                await _appointmentService.DeleteAsync(id);

            return Ok(appointment);
        }
    }
}