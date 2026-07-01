using HealthCareApp.Shared.Enums;
using HealthCareApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using HealthCareApp.Shared.Dtos.Pagination;
using HealthCareApp.Shared.Dtos.Appointments;


namespace HealthCareApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentsController(IAppointmentService service) : ControllerBase
    {

        private const string InvalidUserTokenMessage = "Invalid user token.";
        private const string PatientRoleName = "Patient";
        private const string DoctorRoleName = "Doctor";

        // Admin only: View all appointments
        [HttpGet]
        [Authorize(
     AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
     Roles = "Admin")]
        public async Task<IActionResult> GetAllAppointments([FromQuery] AppointmentPaginationQueryDto query)
        {
            var appointments = await service.GetAllAppointmentsPagedAsync(query);

            return Ok(appointments);
        }

        // Patient/Doctor: View logged-in user's appointments
        [HttpGet("my")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Patient,Doctor")]
        public async Task<IActionResult> GetMyAppointments()
        {
            var identityUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(identityUserId))
            {
                return Unauthorized(new
                {
                    Message = InvalidUserTokenMessage
                });
            }

            if (User.IsInRole(PatientRoleName))
            {
                var appointments = await service.GetMyAppointmentsForPatientAsync(identityUserId);

                return Ok(appointments);
            }

            if (User.IsInRole(DoctorRoleName))
            {
                var appointments = await service.GetMyAppointmentsForDoctorAsync(identityUserId);

                return Ok(appointments);
            }

            return Forbid();
        }

        // Patient/Doctor: View logged-in user's upcoming appointments
        [HttpGet("my/upcoming")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Patient,Doctor")]
        public async Task<IActionResult> GetMyUpcomingAppointments()
        {
            var identityUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(identityUserId))
            {
                return Unauthorized(new
                {
                    Message = InvalidUserTokenMessage
                });
            }

            if (User.IsInRole(PatientRoleName))
            {
                var appointments = await service.GetMyUpcomingAppointmentsForPatientAsync(identityUserId);

                return Ok(appointments);
            }

            if (User.IsInRole(DoctorRoleName))
            {
                var appointments = await service.GetMyUpcomingAppointmentsForDoctorAsync(identityUserId);

                return Ok(appointments);
            }

            return Forbid();
        }

        // Patient/Doctor: View logged-in user's pending appointments
        [HttpGet("my/pending")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Patient,Doctor")]
        public async Task<IActionResult> GetMyPendingAppointments()
        {
            var identityUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(identityUserId))
            {
                return Unauthorized(new
                {
                    Message = InvalidUserTokenMessage
                });
            }

            if (User.IsInRole(PatientRoleName))
            {
                var appointments = await service.GetMyPendingAppointmentsForPatientAsync(identityUserId);

                return Ok(appointments);
            }

            if (User.IsInRole(DoctorRoleName))
            {
                var appointments = await service.GetMyPendingAppointmentsForDoctorAsync(identityUserId);

                return Ok(appointments);
            }

            return Forbid();
        }

        // Doctor only: View logged-in doctor's today's confirmed appointments
        [HttpGet("my/today-confirmed")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Doctor")]
        public async Task<IActionResult> GetMyTodayConfirmedAppointments()
        {
            var identityUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(identityUserId))
            {
                return Unauthorized(new
                {
                    Message = InvalidUserTokenMessage
                });
            }

            var appointments = await service.GetMyTodayConfirmedAppointmentsForDoctorAsync(identityUserId);

            return Ok(appointments);
        }

        // Admin, Patient, Doctor: View appointment by id
        [HttpGet("{appointmentId:int}")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin,Patient,Doctor")]
        public async Task<IActionResult> GetAppointmentById([FromRoute] int appointmentId)
        {
            if (User.IsInRole("Admin"))
            {
                var appointment = await service.GetAppointmentByIdAsync(appointmentId);

                return Ok(appointment);
            }

            if (User.IsInRole(PatientRoleName))
            {
                var identityUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrWhiteSpace(identityUserId))
                {
                    return Unauthorized(new
                    {
                        Message = InvalidUserTokenMessage
                    });
                }

                var appointment = await service.GetAppointmentByIdForPatientAsync(
                    appointmentId,
                    identityUserId);

                return Ok(appointment);
            }

            if (User.IsInRole(DoctorRoleName))
            {
                var identityUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrWhiteSpace(identityUserId))
                {
                    return Unauthorized(new
                    {
                        Message = InvalidUserTokenMessage
                    });
                }

                var appointment = await service.GetAppointmentByIdForDoctorAsync(
                    appointmentId,
                    identityUserId);

                return Ok(appointment);
            }

            return Forbid();
        }

        // Admin only: Get appointments by patient id
        // Patient should use GET /api/Appointments/my
        [HttpGet("patient/{patientId:int}")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin")]
        public async Task<IActionResult> GetAppointmentsByPatientId([FromRoute] int patientId)
        {
            var appointments = await service.GetAppointmentsByPatientIdAsync(patientId);

            return Ok(appointments);
        }

        // Admin only: Get appointments by doctor id
        // Doctor should use GET /api/Appointments/my
        [HttpGet("doctor/{doctorId:int}")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin")]
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

        // Admin only: Upcoming appointments by patient id
        // Patient should use GET /api/Appointments/my/upcoming
        [HttpGet("patient/{patientId:int}/upcoming")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin")]
        public async Task<IActionResult> GetUpcomingAppointmentsByPatientId([FromRoute] int patientId)
        {
            var appointments = await service.GetUpcomingAppointmentsByPatientIdAsync(patientId);

            return Ok(appointments);
        }

        // Admin only: Upcoming appointments by doctor id
        // Doctor should use GET /api/Appointments/my/upcoming
        [HttpGet("doctor/{doctorId:int}/upcoming")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin")]
        public async Task<IActionResult> GetUpcomingAppointmentsByDoctorId([FromRoute] int doctorId)
        {
            var appointments = await service.GetUpcomingAppointmentsByDoctorIdAsync(doctorId);

            return Ok(appointments);
        }

        // Admin only: Pending appointments by patient id
        // Patient should use GET /api/Appointments/my/pending
        [HttpGet("patient/{patientId:int}/pending")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin")]
        public async Task<IActionResult> GetPendingAppointmentsByPatientId([FromRoute] int patientId)
        {
            var appointments = await service.GetPendingAppointmentsByPatientIdAsync(patientId);

            return Ok(appointments);
        }

        // Admin only: Pending appointments by doctor id
        // Doctor should use GET /api/Appointments/my/pending
        [HttpGet("doctor/{doctorId:int}/pending")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin")]
        public async Task<IActionResult> GetPendingAppointmentsByDoctorId([FromRoute] int doctorId)
        {
            var appointments = await service.GetPendingAppointmentsByDoctorIdAsync(doctorId);

            return Ok(appointments);
        }

        // Admin only: Today's confirmed appointments for doctor
        // Doctor should use GET /api/Appointments/my/today-confirmed
        [HttpGet("doctor/{doctorId:int}/today-confirmed")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin")]
        public async Task<IActionResult> GetTodayConfirmedAppointmentsByDoctorId([FromRoute] int doctorId)
        {
            var appointments = await service.GetTodayConfirmedAppointmentsByDoctorIdAsync(doctorId);

            return Ok(appointments);
        }

        [HttpGet("available-slots")]
        [Authorize(
    AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
    Roles = "Patient")]
        public async Task<IActionResult> GetAvailableTimeSlots(
    [FromQuery] int doctorId,
    [FromQuery] DateTime date)
        {
            var slots = await service.GetAvailableTimeSlotsAsync(
                doctorId,
                date);

            return Ok(slots);
        }

        // Patient only: Book appointment
        // PatientId from body is ignored and replaced with logged-in PatientId in service.
        [HttpPost]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Patient")]
        public async Task<IActionResult> BookAppointment([FromBody] BookAppointmentDto request)
        {
            var identityUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(identityUserId))
            {
                return Unauthorized(new
                {
                    Message = InvalidUserTokenMessage
                });
            }

            var appointment = await service.BookAppointmentForPatientAsync(
                request,
                identityUserId);

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

        // Doctor only: Confirm own appointment
        [HttpPut("{appointmentId:int}/confirm")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Doctor")]
        public async Task<IActionResult> ConfirmAppointment([FromRoute] int appointmentId)
        {
            var identityUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(identityUserId))
            {
                return Unauthorized(new
                {
                    Message = InvalidUserTokenMessage
                });
            }

            var appointment = await service.ConfirmAppointmentForDoctorAsync(
                appointmentId,
                identityUserId);

            return Ok(appointment);
        }

        // Doctor only: Complete own appointment
        [HttpPut("{appointmentId:int}/complete")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Doctor")]
        public async Task<IActionResult> CompleteAppointment([FromRoute] int appointmentId)
        {
            var identityUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(identityUserId))
            {
                return Unauthorized(new
                {
                    Message = InvalidUserTokenMessage
                });
            }

            var appointment = await service.CompleteAppointmentForDoctorAsync(
                appointmentId,
                identityUserId);

            return Ok(appointment);
        }

        // Admin, Patient, Doctor: Cancel appointment
        // Patient and Doctor ownership are checked here.
        [HttpPut("cancel")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin,Patient,Doctor")]
        public async Task<IActionResult> CancelAppointment([FromBody] CancelAppointmentDto request)
        {
            if (User.IsInRole("Admin"))
            {
                var appointment = await service.CancelAppointmentAsync(request);

                return Ok(appointment);
            }

            if (User.IsInRole(PatientRoleName))
            {
                var identityUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrWhiteSpace(identityUserId))
                {
                    return Unauthorized(new
                    {
                        Message = InvalidUserTokenMessage
                    });
                }

                var appointment = await service.CancelAppointmentForPatientAsync(
                    request,
                    identityUserId);

                return Ok(appointment);
            }

            if (User.IsInRole(DoctorRoleName))
            {
                var identityUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrWhiteSpace(identityUserId))
                {
                    return Unauthorized(new
                    {
                        Message = InvalidUserTokenMessage   
                    });
                }

                var appointment = await service.CancelAppointmentForDoctorAsync(
                    request,
                    identityUserId);

                return Ok(appointment);
            }

            return Forbid();
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