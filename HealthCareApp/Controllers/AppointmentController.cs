using HealthCareApp.Dtos;
using HealthCareApp.Enums;
using HealthCareApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthCareApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController(IAppointmentService service) : ControllerBase
    {
        // Admin only: View all appointments
        [HttpGet]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin")]
        public async Task<IActionResult> GetAllAppointments()
        {
            var appointments = await service.GetAllAppointmentsAsync();

            return Ok(appointments);
        }

        // Admin, Patient, Doctor: View appointment by id
        [HttpGet("{appointmentId:int}")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin,Patient,Doctor")]
        public async Task<IActionResult> GetAppointmentById([FromRoute] int appointmentId)
        {
            var appointment = await service.GetAppointmentByIdAsync(appointmentId);

            return Ok(appointment);
        }

        // Admin, Patient: Get appointments by patient
        // Later: patient should access only own patientId
        [HttpGet("patient/{patientId:int}")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin,Patient")]
        public async Task<IActionResult> GetAppointmentsByPatientId([FromRoute] int patientId)
        {
            var appointments = await service.GetAppointmentsByPatientIdAsync(patientId);

            return Ok(appointments);
        }

        // Admin, Doctor: Get appointments by doctor
        // Later: doctor should access only own doctorId
        [HttpGet("doctor/{doctorId:int}")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin,Doctor")]
        public async Task<IActionResult> GetAppointmentsByDoctorId([FromRoute] int doctorId)
        {
            var appointments = await service.GetAppointmentsByDoctorIdAsync(doctorId);

            return Ok(appointments);
        }

        // Admin only: Filter appointments by status
        [HttpGet("status/{status}")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin")]
        public async Task<IActionResult> GetAppointmentsByStatus([FromRoute] AppointmentStatus status)
        {
            var appointments = await service.GetAppointmentsByStatusAsync(status);

            return Ok(appointments);
        }

        // Admin only: Get all upcoming appointments
        [HttpGet("upcoming")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin")]
        public async Task<IActionResult> GetUpcomingAppointments()
        {
            var appointments = await service.GetUpcomingAppointmentsAsync();

            return Ok(appointments);
        }

        // Admin, Patient: Upcoming appointments by patient
        [HttpGet("patient/{patientId:int}/upcoming")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin,Patient")]
        public async Task<IActionResult> GetUpcomingAppointmentsByPatientId([FromRoute] int patientId)
        {
            var appointments = await service.GetUpcomingAppointmentsByPatientIdAsync(patientId);

            return Ok(appointments);
        }

        // Admin, Doctor: Upcoming appointments by doctor
        [HttpGet("doctor/{doctorId:int}/upcoming")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin,Doctor")]
        public async Task<IActionResult> GetUpcomingAppointmentsByDoctorId([FromRoute] int doctorId)
        {
            var appointments = await service.GetUpcomingAppointmentsByDoctorIdAsync(doctorId);

            return Ok(appointments);
        }

        // Admin, Patient: Pending appointments by patient
        [HttpGet("patient/{patientId:int}/pending")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin,Patient")]
        public async Task<IActionResult> GetPendingAppointmentsByPatientId([FromRoute] int patientId)
        {
            var appointments = await service.GetPendingAppointmentsByPatientIdAsync(patientId);

            return Ok(appointments);
        }

        // Admin, Doctor: Pending appointments by doctor
        [HttpGet("doctor/{doctorId:int}/pending")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin,Doctor")]
        public async Task<IActionResult> GetPendingAppointmentsByDoctorId([FromRoute] int doctorId)
        {
            var appointments = await service.GetPendingAppointmentsByDoctorIdAsync(doctorId);

            return Ok(appointments);
        }

        // Doctor, Admin: Today's confirmed appointments for doctor
        [HttpGet("doctor/{doctorId:int}/today-confirmed")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin,Doctor")]
        public async Task<IActionResult> GetTodayConfirmedAppointmentsByDoctorId([FromRoute] int doctorId)
        {
            var appointments = await service.GetTodayConfirmedAppointmentsByDoctorIdAsync(doctorId);

            return Ok(appointments);
        }

        // Patient only: Book appointment
        [HttpPost]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Patient")]
        public async Task<IActionResult> BookAppointment([FromBody] BookAppointmentDto request)
        {
            var appointment = await service.BookAppointmentAsync(request);

            return CreatedAtAction(
                nameof(GetAppointmentById),
                new { appointmentId = appointment.AppointmentId },
                appointment);
        }

        // Admin only: Update appointment details
        [HttpPut("{appointmentId:int}")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin")]
        public async Task<IActionResult> UpdateAppointment(
            [FromRoute] int appointmentId,
            [FromBody] UpdateAppointmentDto request)
        {
            var appointment = await service.UpdateAppointmentAsync(appointmentId, request);

            return Ok(appointment);
        }

        // Doctor only: Confirm appointment
        [HttpPut("{appointmentId:int}/confirm")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Doctor")]
        public async Task<IActionResult> ConfirmAppointment([FromRoute] int appointmentId)
        {
            var appointment = await service.ConfirmAppointmentAsync(appointmentId);

            return Ok(appointment);
        }

        // Doctor only: Complete appointment
        [HttpPut("{appointmentId:int}/complete")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Doctor")]
        public async Task<IActionResult> CompleteAppointment([FromRoute] int appointmentId)
        {
            var appointment = await service.CompleteAppointmentAsync(appointmentId);

            return Ok(appointment);
        }

        // Admin, Patient, Doctor: Cancel appointment
        // CancelAppointmentDto should contain AppointmentId and Reason based on your service.
        [HttpPut("cancel")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin,Patient,Doctor")]
        public async Task<IActionResult> CancelAppointment([FromBody] CancelAppointmentDto request)
        {
            var appointment = await service.CancelAppointmentAsync(request);

            return Ok(appointment);
        }

        // Admin only: Delete appointment
        [HttpDelete("{appointmentId:int}")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin")]
        public async Task<IActionResult> DeleteAppointment([FromRoute] int appointmentId)
        {
            var appointment = await service.DeleteAppointmentAsync(appointmentId);

            return Ok(appointment);
        }
    }
}