using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using S3_HealthAxis.Shared.DTOs.Appointment;
using S3_HealthAxisApi.Services.Interface;

namespace S3_HealthAxisApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentsController(
            IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<AppointmentDetailsDto>>> GetAll()
        {
            var appointments = await _appointmentService.GetAllAsync();
            return Ok(appointments);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var appointment =
                await _appointmentService.GetByIdAsync(id);

            if (appointment == null)
                return NotFound(
                    $"Appointment {id} not found.");

            return Ok(appointment);
        }

        [HttpGet("patient/{patientId:int}")]
        public async Task<IActionResult> GetPatientHistory(
            int patientId)
        {
            var appointments =
                await _appointmentService
                    .GetPatientHistoryAsync(patientId);

            return Ok(appointments);
        }

        [HttpGet("doctor/{doctorId:int}/today")]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<IActionResult> GetDoctorTodaySchedule(
            int doctorId)
        {
            var schedule =
                await _appointmentService
                    .GetDoctorTodayScheduleAsync(doctorId);

            return Ok(schedule);
        }

        [HttpGet("doctor/{doctorId:int}/week")]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<IActionResult> GetDoctorWeekSchedule(
            int doctorId,
            [FromQuery] DateOnly startDate,
            [FromQuery] DateOnly endDate)
        {
            var schedule =
                await _appointmentService
                    .GetDoctorWeekScheduleAsync(
                        doctorId,
                        startDate,
                        endDate);

            return Ok(schedule);
        }

        [HttpGet("doctor/{doctorId}/upcoming")]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> GetDoctorUpcomingSchedule(int doctorId)
        {
            var result =
                await _appointmentService
                    .GetDoctorUpcomingScheduleAsync(
                        doctorId);

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Patient,Admin")]
        public async Task<IActionResult> Create(
            [FromBody] CreateAppointmentDto dto)
        {
            try
            {
                var appointment =
                    await _appointmentService.CreateAsync(dto);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = appointment.AppointmentId },
                    appointment);
            }
            catch (
                ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (
                InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (
                KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Patient,Admin")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateAppointmentDto dto)
        {
            try
            {
                await _appointmentService
                    .UpdateAsync(id, dto);

                return NoContent();
            }
            catch (
                KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (
                ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (
                InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:int}/confirm")]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<IActionResult> Confirm(int id)
        {
            try
            {
                await _appointmentService
                    .ConfirmAsync(id);

                return NoContent();
            }
            catch (
                KeyNotFoundException)
            {
                return NotFound();
            }
            catch (
                InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:int}/complete")]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<IActionResult> Complete(int id)
        {
            try
            {
                await _appointmentService
                    .CompleteAsync(id);

                return NoContent();
            }
            catch (
                KeyNotFoundException)
            {
                return NotFound();
            }
            catch (
                InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/status")]
        [Authorize]
        public async Task<IActionResult> UpdateStatus(int id, UpdateAppointmentStatusDto dto)
        {
            await _appointmentService
                .UpdateStatusAsync(id, dto);

            return NoContent();
        }

        [HttpPut("{id:int}/cancel")]
        [Authorize(Roles = "Patient,Admin")]
        public async Task<IActionResult> Cancel(
            int id,
            [FromBody] CancelAppointmentDto dto)
        {
            try
            {
                await _appointmentService
                    .CancelAsync(id, dto);

                return NoContent();
            }
            catch (
                KeyNotFoundException)
            {
                return NotFound();
            }
            catch (
                ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (
                InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}