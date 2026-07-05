using Healthcare.Shared.DTOs.Appointments;
using HealthCare.Api.Services.Implementations;
using HealthCare.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthCare.Api.Controllers
{
    
    [Route("api/appointments")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _service;

        public AppointmentController(IAppointmentService service)
        {
            _service = service;
        }


        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Patient")]
        public async Task<IActionResult> Create([FromBody] CreateAppointmentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var patientId = GetPatientIdFromClaims();

            await _service.AddAsync(dto, patientId);

            return StatusCode(201, new
            {
                message = "Appointment created successfully"
            });
        }


        [HttpPut("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Doctor")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAppointmentDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _service.UpdateAsync(id, dto);

            return NoContent();
        }


        [HttpPatch("{id:int}/status")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Doctor")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateAppointmentDto dto)
        {
            await _service.UpdateStatusAsync(id, dto);
            return NoContent();
        }


        [HttpGet("available-slots")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Patient")]
        public async Task<IActionResult> GetAvailableSlots( [FromQuery] int doctorId,[FromQuery] DateOnly date)
        {
            var slots = await _service.AvailableTimeSlots(date, doctorId);
            return Ok(slots);
        }


        [HttpGet("check-availability")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Patient")]
        public async Task<IActionResult> CheckAvailability(int doctorId, DateOnly date, string timeSlot)
        {
            var result = await _service.IsAvailable(date, doctorId, timeSlot);
            return Ok(new { available = result });
        }

        [HttpGet("doctor/schedule")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Doctor")]
        public async Task<IActionResult> GetDoctorSchedule( DateOnly date)
        {
            var doctorId = GetDoctorIdFromClaims();
            var result = await _service.GetDoctorSchedule(date, doctorId);
            return Ok(result);
        }


        [HttpGet("my-schedule")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Doctor")]
        public async Task<IActionResult> GetMySchedule([FromQuery] DateOnly date)
        {
            var patientId = GetPatientIdFromClaims();
            var result = await _service.GetPatientSchedule(date, patientId);
            return Ok(result);
        }

        [HttpGet("my")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Patient")]
        public async Task<IActionResult> GetMyAppointments()
        {
            var patientId = GetPatientIdFromClaims();
            var result = await _service.GetAppointmentByPatient(patientId);
            return Ok(result);
        }


        [HttpGet("doctor")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetDoctorAppointments()
        {
            var doctorId = GetDoctorIdFromClaims();
            var result = await _service.GetAppointmentByDoctor(doctorId);
            return Ok(result);
        }

        private int GetPatientIdFromClaims()
        {
            var claim = User.FindFirst("PatientId")
                ?? throw new InvalidOperationException("PatientId claim not found in token.");

            return int.Parse(claim.Value);
        }

        private int GetDoctorIdFromClaims()
        {
            var claim = User.FindFirst("DoctorId")
                ?? throw new InvalidOperationException("DoctorId claim not found in token.");

            return int.Parse(claim.Value);
        }

    }
}
