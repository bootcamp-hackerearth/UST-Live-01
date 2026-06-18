using HealthApp.Api.Dtos;
using HealthApp.Api.Enums;
using HealthApp.Api.Exceptions;
using HealthApp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HealthApp.Api.Controllers
{
    [ApiController]
    [Route("api/appointments")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IHealthRecordService _healthRecordService;

        public AppointmentsController(
            IAppointmentService appointmentService,
            IHealthRecordService healthRecordService)
        {
            _appointmentService = appointmentService;
            _healthRecordService = healthRecordService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAppointments(
            [FromQuery] int? doctorId,
            [FromQuery] int? patientId,
            [FromQuery] bool onlyUpcoming = false)
        {
            var appointments = await _appointmentService.GetAppointmentsAsync(
                doctorId,
                patientId,
                onlyUpcoming);

            return Ok(appointments);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAppointmentById(int id)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);

            return Ok(appointment);
        }

        [HttpPost]
        public async Task<IActionResult> BookAppointment(
            [FromBody] AppointmentCreateDto dto)
        {
            var appointment = await _appointmentService.BookAppointmentAsync(dto);

            return StatusCode(
                StatusCodes.Status201Created,
                appointment);
        }

        [HttpPut("{id:int}/status")]
        public async Task<IActionResult> UpdateAppointmentStatus(
            int id,
            [FromQuery] string status,
            [FromQuery] string? cancellationReason)
        {
            if (!Enum.TryParse(
                    status,
                    true,
                    out AppointmentStatus appointmentStatus))
            {
                throw new InvalidRequestException("Invalid appointment status.");
            }

            await _appointmentService.UpdateAppointmentStatusAsync(
                id,
                appointmentStatus,
                cancellationReason);

            return Ok(new { message = "Appointment status updated successfully." });
        }

        [HttpPost("{id:int}/confirm")]
        public async Task<IActionResult> ConfirmAppointment(int id)
        {
            await _appointmentService.UpdateAppointmentStatusAsync(
                id,
                AppointmentStatus.Confirmed);

            return Ok(new { message = "Appointment confirmed successfully." });
        }

        [HttpPost("{id:int}/cancel")]
        public async Task<IActionResult> CancelAppointment(
            int id,
            [FromBody] CancelAppointmentDto dto)
        {
            await _appointmentService.UpdateAppointmentStatusAsync(
                id,
                AppointmentStatus.Cancelled,
                dto.CancellationReason);

            return Ok(new { message = "Appointment cancelled successfully." });
        }

        [HttpPost("{id:int}/complete")]
        public async Task<IActionResult> CompleteAppointment(int id)
        {
            await _appointmentService.UpdateAppointmentStatusAsync(
                id,
                AppointmentStatus.Completed);

            return Ok(new { message = "Appointment completed successfully." });
        }

        [HttpGet("patient/{patientId:int}")]
        public async Task<IActionResult> GetAppointmentsByPatient(int patientId)
        {
            var appointments = await _appointmentService.GetAppointmentsAsync(
                null,
                patientId,
                false);

            return Ok(appointments);
        }

        [HttpGet("doctor/{doctorId:int}")]
        public async Task<IActionResult> GetAppointmentsByDoctor(
            int doctorId,
            [FromQuery] bool onlyUpcoming = false)
        {
            var appointments = await _appointmentService.GetAppointmentsAsync(
                doctorId,
                null,
                onlyUpcoming);

            return Ok(appointments);
        }

        [HttpGet("upcoming")]
        public async Task<IActionResult> GetUpcomingAppointments()
        {
            var appointments = await _appointmentService.GetAppointmentsAsync(
                null,
                null,
                true);

            return Ok(appointments);
        }

        [HttpGet("slots")]
        public async Task<IActionResult> GetAvailableSlots(
            [FromQuery] int doctorId,
            [FromQuery] DateOnly date)
        {
            var slots = await _appointmentService.GetAvailableSlotsAsync(
                doctorId,
                date);

            return Ok(slots);
        }

        [HttpGet("{id:int}/healthrecord")]
        public async Task<IActionResult> HealthRecordExists(int id)
        {
            var exists = await _healthRecordService.ExistsByAppointmentIdAsync(id);

            return Ok(exists);
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            await _appointmentService.DeleteAppointmentAsync(id);

            return Ok(new
            {
                message = "Appointment deleted successfully."
            });
        }

    }
}
