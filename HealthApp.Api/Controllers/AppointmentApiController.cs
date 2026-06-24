using HealthApp.Api.Dto;
using HealthApp.Api.Service.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthApp.Api.Controllers
{
    [Route("api/appointments")]
    [ApiController]
    public class AppointmentApiController : ControllerBase
    {
        private readonly IAppointmentService _service;

        public AppointmentApiController(IAppointmentService service)
        {
            _service = service;
        }

        // GET ALL
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllAppointments();
            return Ok(data);
        }

        // GET BY ID
        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _service.GetAppointmentById(id);
            return Ok(data);
        }

        // CREATE
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        public async Task<IActionResult> Add([FromBody] AppointmentDto dto)
        {
            await _service.Add(dto);
            return Ok("Appointment created successfully");
        }

        // CANCEL
        [HttpPut("{id}/cancel")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User,Doctor,Admin")]
        public async Task<IActionResult> Cancel(int id, [FromQuery] string reason)
        {
            await _service.CancelAppointment(id, reason);
            return Ok("Appointment cancelled");
        }

        // CONFIRM
        [HttpPut("{id}/confirm")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Doctor")]
        public async Task<IActionResult> Confirm(int id)
        {
            await _service.ConfirmAppointment(id);
            return Ok("Appointment confirmed");
        }

        // COMPLETE
        [HttpPut("{id}/complete")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Doctor")]
        public async Task<IActionResult> Complete(int id)
        {
            await _service.CompleteAppointment(id);
            return Ok("Appointment completed");
        }

        // CHECK DOCTOR AVAILABILITY
        [HttpGet("doctor/{doctorId}/availability")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User,Admin")]
        public async Task<IActionResult> CheckDoctorAvailability(int doctorId, [FromQuery] DateTime date)
        {
            var data = await _service.CheckDoctorAvailability(doctorId, date);
            return Ok(data);
        }

        // CHECK IF SLOT IS BOOKED
        [HttpGet("slot-booked")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User,Admin")]
        public async Task<IActionResult> IsSlotBooked([FromQuery] int doctorId, [FromQuery] DateTime date, [FromQuery] string timeSlot)
        {
            var data = await _service.IsSlotBooked(doctorId, date, timeSlot);
            return Ok(data);
        }

        // GET UPCOMING APPOINTMENTS BY DOCTOR
        [HttpGet("doctor/{doctorId}/upcoming")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Doctor,Admin")]
        public async Task<IActionResult> GetUpcomingAppointmentsByDoctor(
            int doctorId,
            [FromQuery] DateTime fromDate,
            [FromQuery] DateTime toDate)
        {
            var data = await _service.GetUpcomingAppointmentsByDoctor(doctorId, fromDate, toDate);
            return Ok(data);
        }

        // GET APPOINTMENTS BY PATIENT AND DOCTOR
        [HttpGet("by-patient-doctor")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User,Doctor,Admin")]
        public async Task<IActionResult> GetAppointmentsByPatientAndDoctor(
            [FromQuery] int patientId,
            [FromQuery] int doctorId)
        {
            var data = await _service.GetAppointmentsByPatientAndDoctor(patientId, doctorId);
            return Ok(data);
        }
    }
}