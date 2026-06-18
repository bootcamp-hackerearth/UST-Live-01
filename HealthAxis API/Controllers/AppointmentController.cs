using HealthAxis.API.DTOs.Appointments;
using HealthAxis.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxis.API.Controllers
{
    [Route("api/appointments")]
    [ApiController]
    [Authorize(Roles = "Patient,Doctor,Admin")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentsController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var appointments =
                await _appointmentService.GetAllAsync(ct);

            return Ok(appointments);
        }

        [HttpPost]
        [Authorize(Roles = "Patient,Admin")]
        public async Task<IActionResult> Create(
            AppointmentCreateDto request,
            CancellationToken ct)
        {
            var appointment =
                await _appointmentService.CreateAsync(request, ct);

            return Ok(appointment);
        }

        [HttpPut("{id:int}/status")]
        public async Task<IActionResult> UpdateStatus(
            int id,
            AppointmentStatusUpdateDto request,
            CancellationToken ct)
        {
            var appointment =
                await _appointmentService.UpdateStatusAsync(id, request, ct);

            if (appointment == null)
            {
                return NotFound(new { message = "Appointment not found." });
            }

            return Ok(appointment);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Patient,Admin")]
        public async Task<IActionResult> Delete(
            int id,
            CancellationToken ct)
        {
            var appointment =
                await _appointmentService.DeleteAsync(id, ct);

            if (appointment == null)
            {
                return NotFound(new { message = "Appointment not found." });
            }

            return Ok(appointment);
        }
    }
}
