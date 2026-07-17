using HealthAxis.API.Services;
using HealthAxis.API.Services.Interfaces;
using HealthAxis.Shared.DTO.AppointmentDtos;
using HealthAxis.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthAxis.API.Controller
{
    [Route("api/appointments")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private const string InvalidToken = "Invalid token";

        private const string PatientProfileNotFound =
            "Patient profile not found";

        private const string DoctorProfileNotFound =
            "Doctor profile not found";

        private const string AppointmentNotFound =
            "Appointment not found";

        private readonly IAppointmentService _appointmentService;

        private readonly IPatientService _patientService;

        private readonly IDoctorService _doctorService;

        public AppointmentController(
            IAppointmentService appointmentService,
            IPatientService patientService,
            IDoctorService doctorService)
        {
            _appointmentService = appointmentService;
            _patientService = patientService;
            _doctorService = doctorService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllAppointments()
        {
            var appointments = await _appointmentService.GetAllAsync();

            return Ok(appointments);
        }

        [HttpGet("my")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> GetMyAppointments()
        {
            var userId = GetLoggedInUserId();

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized(new
                {
                    message = InvalidToken
                });
            }

            var patient = await _patientService.GetByUserIdAsync(userId);

            if (patient == null)
            {
                return NotFound(new
                {
                    message = PatientProfileNotFound
                });
            }

            var appointments = await _appointmentService.GetByPatientIdAsync(
                patient.PatientId);

            return Ok(appointments);
        }

        [HttpGet("doctor/my")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetMyDoctorAppointments()
        {
            var userId = GetLoggedInUserId();

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized(new
                {
                    message = InvalidToken
                });
            }

            var doctor = await _doctorService.GetByUserIdAsync(userId);

            if (doctor == null)
            {
                return NotFound(new
                {
                    message = DoctorProfileNotFound
                });
            }

            var appointments = await _appointmentService.GetByDoctorIdAsync(
                doctor.DoctorId);

            return Ok(appointments);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Patient,Doctor,Admin")]
        public async Task<IActionResult> GetAppointmentById(int id)
        {
            var appointment = await _appointmentService.GetByIdAsync(id);

            if (appointment == null)
            {
                return NotFound(new
                {
                    message = AppointmentNotFound
                });
            }

            var accessResult = await ValidateAppointmentAccessAsync(
                appointment);

            if (accessResult != null)
            {
                return accessResult;
            }

            return Ok(appointment);
        }

        [HttpPost]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> CreateAppointment(
            [FromBody] CreateAppointmentDto appointmentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = GetLoggedInUserId();

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized(new
                {
                    message = InvalidToken
                });
            }

            var patient = await _patientService.GetByUserIdAsync(userId);

            if (patient == null)
            {
                return NotFound(new
                {
                    message = PatientProfileNotFound
                });
            }

            appointmentDto.PatientId = patient.PatientId;

            var appointment = await _appointmentService.AddAsync(
                appointmentDto);

            return Ok(appointment);
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "Patient,Doctor,Admin")]
        public async Task<IActionResult> UpdateAppointmentStatus(
            int id,
            [FromBody] UpdateAppointmentStatusDto statusDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var appointment = await _appointmentService.GetByIdAsync(id);

            var appointmentValidationResult =
                ValidateAppointmentForStatusUpdate(appointment);

            if (appointmentValidationResult != null)
            {
                return appointmentValidationResult;
            }

            var permissionResult =
                await ValidateStatusUpdatePermissionAsync(
                    appointment!,
                    statusDto);

            if (permissionResult != null)
            {
                return permissionResult;
            }

            NormalizeCancellationReason(statusDto);

            var updatedAppointment = await _appointmentService.UpdateStatusAsync(
                id,
                statusDto);

            return Ok(updatedAppointment);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var appointment = await _appointmentService.DeleteAsync(id);

            return Ok(appointment);
        }

        private string? GetLoggedInUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        private async Task<IActionResult?> ValidateAppointmentAccessAsync(
            AppointmentDto appointment)
        {
            var userId = GetLoggedInUserId();

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized(new
                {
                    message = InvalidToken
                });
            }

            if (User.IsInRole("Patient"))
            {
                return await ValidatePatientAppointmentAccessAsync(
                    appointment,
                    userId);
            }

            if (User.IsInRole("Doctor"))
            {
                return await ValidateDoctorAppointmentAccessAsync(
                    appointment,
                    userId);
            }

            return null;
        }

        private async Task<IActionResult?> ValidatePatientAppointmentAccessAsync(
            AppointmentDto appointment,
            string userId)
        {
            var patient = await _patientService.GetByUserIdAsync(userId);

            if (patient == null)
            {
                return NotFound(new
                {
                    message = PatientProfileNotFound
                });
            }

            if (appointment.PatientId != patient.PatientId)
            {
                return StatusCode(403, new
                {
                    message = "You are not allowed to access another patient's appointment"
                });
            }

            return null;
        }

        private async Task<IActionResult?> ValidateDoctorAppointmentAccessAsync(
            AppointmentDto appointment,
            string userId)
        {
            var doctor = await _doctorService.GetByUserIdAsync(userId);

            if (doctor == null)
            {
                return NotFound(new
                {
                    message = DoctorProfileNotFound
                });
            }

            if (appointment.DoctorId != doctor.DoctorId)
            {
                return StatusCode(403, new
                {
                    message = "You are not allowed to access another doctor's appointment"
                });
            }

            return null;
        }

        private IActionResult? ValidateAppointmentForStatusUpdate(
            AppointmentDto? appointment)
        {
            if (appointment == null)
            {
                return NotFound(new
                {
                    message = AppointmentNotFound
                });
            }

            if (appointment.Status == AppointmentStatus.Completed ||
                appointment.Status == AppointmentStatus.Cancelled)
            {
                return BadRequest(new
                {
                    message = "Completed or cancelled appointment cannot be changed"
                });
            }

            return null;
        }

        private async Task<IActionResult?> ValidateStatusUpdatePermissionAsync(
            AppointmentDto appointment,
            UpdateAppointmentStatusDto statusDto)
        {
            var userId = GetLoggedInUserId();

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized(new
                {
                    message = InvalidToken
                });
            }

            if (User.IsInRole("Patient"))
            {
                return await ValidatePatientStatusUpdateAsync(
                    appointment,
                    statusDto,
                    userId);
            }

            if (User.IsInRole("Doctor"))
            {
                return await ValidateDoctorStatusUpdateAsync(
                    appointment,
                    statusDto,
                    userId);
            }

            return null;
        }

        private async Task<IActionResult?> ValidatePatientStatusUpdateAsync(
            AppointmentDto appointment,
            UpdateAppointmentStatusDto statusDto,
            string userId)
        {
            var patient = await _patientService.GetByUserIdAsync(userId);

            if (patient == null)
            {
                return NotFound(new
                {
                    message = PatientProfileNotFound
                });
            }

            if (appointment.PatientId != patient.PatientId)
            {
                return StatusCode(403, new
                {
                    message = "You cannot update another patient's appointment"
                });
            }

            if (statusDto.Status != AppointmentStatus.Cancelled)
            {
                return StatusCode(403, new
                {
                    message = "Patient can only cancel appointment"
                });
            }

            return null;
        }

        private async Task<IActionResult?> ValidateDoctorStatusUpdateAsync(
            AppointmentDto appointment,
            UpdateAppointmentStatusDto statusDto,
            string userId)
        {
            var doctor = await _doctorService.GetByUserIdAsync(userId);

            if (doctor == null)
            {
                return NotFound(new
                {
                    message = DoctorProfileNotFound
                });
            }

            if (appointment.DoctorId != doctor.DoctorId)
            {
                return StatusCode(403, new
                {
                    message = "You cannot update another doctor's appointment"
                });
            }

            if (statusDto.Status == AppointmentStatus.Confirmed ||
                statusDto.Status == AppointmentStatus.Cancelled)
            {
                return ValidateDoctorCancellationReason(statusDto);
            }

            return StatusCode(403, new
            {
                message = "Doctor can only confirm or cancel appointment here. Completion happens after adding health record"
            });
        }

        private BadRequestObjectResult? ValidateDoctorCancellationReason(
            UpdateAppointmentStatusDto statusDto)
        {
            if (statusDto.Status != AppointmentStatus.Cancelled)
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(statusDto.CancellationReason))
            {
                return BadRequest(new
                {
                    message = "Cancellation reason is required."
                });
            }

            return null;
        }

        private void NormalizeCancellationReason(
      UpdateAppointmentStatusDto statusDto)
        {
            if (statusDto.Status != AppointmentStatus.Cancelled)
            {
                statusDto.CancellationReason = null;
                return;
            }

            var reason = string.IsNullOrWhiteSpace(statusDto.CancellationReason)
                ? string.Empty
                : statusDto.CancellationReason.Trim();

            if (User.IsInRole("Patient"))
            {
                statusDto.CancellationReason = string.IsNullOrWhiteSpace(reason)
                    ? "Cancelled by patient."
                    : $"Cancelled by patient. Reason: {reason}";

                return;
            }

            if (User.IsInRole("Doctor"))
            {
                statusDto.CancellationReason = string.IsNullOrWhiteSpace(reason)
                    ? "Cancelled by doctor."
                    : $"Cancelled by doctor. Reason: {reason}";

                return;
            }

            if (User.IsInRole("Admin"))
            {
                statusDto.CancellationReason = string.IsNullOrWhiteSpace(reason)
                    ? "Cancelled by admin."
                    : $"Cancelled by admin. Reason: {reason}";
            }
        }
    }
}