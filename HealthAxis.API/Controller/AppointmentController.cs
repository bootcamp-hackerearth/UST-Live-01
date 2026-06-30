using HealthAxis.Shared.DTO.AppointmentDtos;
using HealthAxis.Shared.Enums;
using HealthAxis.API.Services;
using HealthAxis.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthAxis.API.Controller
{
    [Route("api/appointments")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
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

        private string? GetLoggedInUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
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
                    message = "Invalid token"
                });
            }

            var patient = await _patientService.GetByUserIdAsync(userId);

            if (patient == null)
            {
                return NotFound(new
                {
                    message = "Patient profile not found"
                });
            }

            var appointments = await _appointmentService.GetByPatientIdAsync(
                patient.PatientId);

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
                    message = "Appointment not found"
                });
            }

            var userId = GetLoggedInUserId();

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized(new
                {
                    message = "Invalid token"
                });
            }

            if (User.IsInRole("Patient"))
            {
                var patient = await _patientService.GetByUserIdAsync(userId);

                if (patient == null)
                {
                    return NotFound(new
                    {
                        message = "Patient profile not found"
                    });
                }

                if (appointment.PatientId != patient.PatientId)
                {
                    return StatusCode(403, new
                    {
                        message = "You are not allowed to access another patient's appointment"
                    });
                }
            }

            if (User.IsInRole("Doctor"))
            {
                var doctor = await _doctorService.GetByUserIdAsync(userId);

                if (doctor == null)
                {
                    return NotFound(new
                    {
                        message = "Doctor profile not found"
                    });
                }

                if (appointment.DoctorId != doctor.DoctorId)
                {
                    return StatusCode(403, new
                    {
                        message = "You are not allowed to access another doctor's appointment"
                    });
                }
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
                    message = "Invalid token"
                });
            }

            var patient = await _patientService.GetByUserIdAsync(userId);

            if (patient == null)
            {
                return NotFound(new
                {
                    message = "Patient profile not found"
                });
            }

            appointmentDto.PatientId = patient.PatientId;

            var appointment = await _appointmentService.AddAsync(appointmentDto);

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

            if (appointment == null)
            {
                return NotFound(new
                {
                    message = "Appointment not found"
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

            var userId = GetLoggedInUserId();

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized(new
                {
                    message = "Invalid token"
                });
            }

            if (User.IsInRole("Patient"))
            {
                var patient = await _patientService.GetByUserIdAsync(userId);

                if (patient == null)
                {
                    return NotFound(new
                    {
                        message = "Patient profile not found"
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

                statusDto.CancellationReason =
                    string.IsNullOrWhiteSpace(statusDto.CancellationReason)
                        ? null
                        : statusDto.CancellationReason.Trim();
            }

            if (User.IsInRole("Doctor"))
            {
                var doctor = await _doctorService.GetByUserIdAsync(userId);

                if (doctor == null)
                {
                    return NotFound(new
                    {
                        message = "Doctor profile not found"
                    });
                }

                if (appointment.DoctorId != doctor.DoctorId)
                {
                    return StatusCode(403, new
                    {
                        message = "You cannot update another doctor's appointment"
                    });
                }

                if (statusDto.Status != AppointmentStatus.Confirmed)
                {
                    return StatusCode(403, new
                    {
                        message = "Doctor can only confirm appointment here. Completion happens after adding health record"
                    });
                }
            }

            var updatedAppointment = await _appointmentService.UpdateStatusAsync(
                id,
                statusDto);

            return Ok(updatedAppointment);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Patient")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var appointment = await _appointmentService.DeleteAsync(id);

            return Ok(appointment);
        }
    }
}