using HealthAxisHealth.Shared.DTOs.AppointmentDtos;
using HealthAxisHealth.API.Helpers;
using HealthAxisHealth.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using HealthAxisHealth.Shared.Utilities;
using System.Diagnostics.CodeAnalysis;

namespace HealthAxisHealth.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    [ExcludeFromCodeCoverage]
    public class AppointmentsController :
        ControllerBase
    {
        #region Fields

        private readonly IAppointmentService
            _appointmentService;

        #endregion

        #region Constructor

        public AppointmentsController(
            IAppointmentService appointmentService)
        {
            _appointmentService =
                appointmentService;
        }

        #endregion

        #region Properties

        private int CurrentUserId =>
            int.Parse(
                User.FindFirst(
                    ClaimTypes.NameIdentifier)!
                .Value);

        #endregion

        #region Endpoints

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult>
            GetAppointments(
                [FromQuery]
                PaginationParams pagination)
        {
            var appointments =
                await _appointmentService
                    .GetPagedAsync(
                        pagination);

            return Ok(appointments);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult>
            GetById(
                int id)
        {
            var appointment =
                await _appointmentService
                    .GetByIdAsync(id);

            return Ok(appointment);
        }

        [HttpGet("patient/{patientId:int}")]
        [Authorize(Roles = "Patient,Admin")]
        public async Task<IActionResult>
            GetByPatient(
                int patientId)
        {
            var appointments =
                await _appointmentService
                    .GetByPatientIdAsync(
                        patientId);

            return Ok(appointments);
        }

        [HttpGet("doctor/{doctorId:int}")]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<IActionResult>
            GetByDoctor(
                int doctorId)
        {
            var appointments =
                await _appointmentService
                    .GetByDoctorIdAsync(
                        doctorId);

            return Ok(appointments);
        }

        [HttpGet("doctor")]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<IActionResult>
            GetDoctorAppointments(
                [FromQuery] int doctorId,
                [FromQuery] DateTime date)
        {
            var appointments =
                await _appointmentService
                    .GetDoctorAppointmentsByDateAsync(
                        doctorId,
                        date);

            return Ok(appointments);
        }

        [HttpPost]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult>
            Create(
                CreateAppointmentDto dto)
        {
            int appointmentId =
                await _appointmentService
                    .CreateAsync(
                        CurrentUserId,
                        dto);

            return Ok(new { AppointmentId = appointmentId, Message = Constants.AppointmentBookedSuccessfully });
        }

        [HttpPut("{id:int}/status")]
        [Authorize(Roles = "Patient,Doctor,Admin")]
        public async Task<IActionResult>
            UpdateStatus(
                int id,
                UpdateAppointmentStatusDto dto)
        {
            await _appointmentService
                .UpdateStatusAsync(
                    id,
                    dto);

            return Ok(new { Message = Constants.AppointmentStatusUpdatedSuccessfully });
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Patient,Admin")]
        public async Task<IActionResult>
            Delete(
                int id)
        {
            await _appointmentService
                .DeleteAsync(id);

            return Ok(new { Message = Constants.AppointmentDeletedSuccessfully });
        }

        #endregion
    }
}
