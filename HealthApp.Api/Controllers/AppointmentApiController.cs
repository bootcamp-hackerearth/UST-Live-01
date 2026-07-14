using HealthApp.Api.Service.Impl;
using HealthApp.Api.Service.Interface;
using HealthApp.Shared.Dto;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _service.GetAppointmentById(id);
            return Ok(data);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        public async Task<IActionResult> Add([FromBody] AppointmentDto dto)
        {
            var identityUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(identityUserId))
                return Unauthorized(new { message = "Invalid token." });

            var result = await _service.Add(dto, identityUserId);

            return Ok(result);
        }


        [HttpPut("{id}/cancel")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User,Doctor")]
        public async Task<IActionResult> Cancel(int id, [FromQuery] string reason)
        {
            var result = await _service.CancelAppointment(id, reason);
            return Ok(result);
        }

        [HttpPut("{id}/confirm")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Doctor")]
        public async Task<IActionResult> Confirm(int id)
        {
            var result = await _service.ConfirmAppointment(id);
            return Ok(result);
        }

        [HttpPut("{id}/complete")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Doctor")]
        public async Task<IActionResult> Complete(int id)
        {
            var result = await _service.CompleteAppointment(id);
            return Ok(result);
        }


        [HttpGet("doctor/{doctorId:int}/availability")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CheckDoctorAvailability(int doctorId,[FromQuery] DateTime date)
        {
            var result = await _service
                .CheckDoctorAvailability(doctorId, date);

            return Ok(result);
        }

        [HttpGet("slot-booked")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        public async Task<IActionResult> IsSlotBooked(
            int doctorId,
            DateTime date,
            string timeSlot)
        {
            var data = await _service.IsSlotBooked(doctorId, date, timeSlot);
            return Ok(data);
        }

        [HttpGet("doctor/{doctorId}/upcoming")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Doctor,Admin")]
        public async Task<IActionResult> GetUpcomingAppointmentsByDoctor(
            int doctorId,
            [FromQuery] DateTime fromDate,
            [FromQuery] DateTime toDate)
        {
            var data = await _service.GetUpcomingAppointmentsByDoctor(
                doctorId, fromDate, toDate);

            return Ok(data);
        }

        [HttpGet("paged")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> GetPaged(
            int pageNumber = 1,
            int pageSize = 10)
        {
            var (data, total) =
                await _service.GetPagedAppointments(pageNumber, pageSize);

            return Ok(new
            {
                data,
                totalRecords = total,
                pageNumber,
                pageSize
            });
        }

        [HttpGet("filter-paged")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User,Doctor,Admin")]
        public async Task<IActionResult> GetFilteredPaged(
            [FromQuery] int? patientId,
            [FromQuery] int? doctorId,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var (data, total) =
                await _service.GetAppointmentsByPatientAndDoctorPaged(
                    patientId,
                    doctorId,
                    pageNumber,
                    pageSize);

            return Ok(new
            {
                data,
                totalRecords = total,
                pageNumber,
                pageSize
            });
        }


        [HttpGet("me")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        public async Task<IActionResult> GetMyAppointments()
        {
            var identityUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(identityUserId))
                return Unauthorized(new { message = "Invalid token." });

            var data = await _service.GetAppointmentsByUserAsync(identityUserId);

            return Ok(data);
        }



        [HttpGet("doctor/me")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Doctor")]
        public async Task<IActionResult> GetMyDoctorAppointments()
        {
            var identityUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(identityUserId))
                return Unauthorized(new { message = "Invalid token." });

            var data = await _service.GetAppointmentsByDoctorAsync(identityUserId);

            return Ok(data);
        }

    }
}