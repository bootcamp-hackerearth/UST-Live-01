using HealthApp.Api.Dtos;
using HealthApp.Api.Enums;
using HealthApp.Api.Exceptions;
using HealthApp.Api.Extensions;
using HealthApp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthApp.Api.Controllers
{
    [ApiController]
    [Route("api/appointments")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        private readonly string PatientNotLinked = "Patient profile is not linked to this user.";
        private readonly string DoctorNotLinked = "Doctor profile is not linked to this user.";

        public AppointmentsController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> GetAppointmentsForAdmin(
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

        [HttpGet("my")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Patient,Doctor")]
        public async Task<IActionResult> GetMyAppointments([FromQuery] bool onlyUpcoming = false)
        {
            int? patientId = null;
            int? doctorId = null;

            if (User.IsPatient())
            {
                patientId = User.GetPatientId();

                if (patientId == null)
                {
                    throw new ForbiddenAccessException(PatientNotLinked);
                }
            }

            if (User.IsDoctor())
            {
                doctorId = User.GetDoctorId();

                if (doctorId == null)
                {
                    throw new ForbiddenAccessException(DoctorNotLinked);
                }
            }

            var appointments = await _appointmentService.GetAppointmentsAsync(
                doctorId,
                patientId,
                onlyUpcoming);

            return Ok(appointments);
        }

        [HttpGet("my/upcoming")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Patient,Doctor")]
        public async Task<IActionResult> GetMyUpcomingAppointments()
        {
            int? patientId = null;
            int? doctorId = null;

            if (User.IsPatient())
            {
                patientId = User.GetPatientId();

                if (patientId == null)
                {
                    throw new ForbiddenAccessException(PatientNotLinked);
                }
            }

            if (User.IsDoctor())
            {
                doctorId = User.GetDoctorId();

                if (doctorId == null)
                {
                    throw new ForbiddenAccessException(DoctorNotLinked);
                }
            }

            var appointments = await _appointmentService.GetAppointmentsAsync(
                doctorId,
                patientId,
                true);

            return Ok(appointments);
        }

        [HttpGet("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Patient,Doctor,Admin")]
        public async Task<IActionResult> GetAppointmentById(int id)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);

            if (User.IsPatient() && appointment.PatientId != User.GetPatientId())
            {
                throw new ForbiddenAccessException(
                    "You cannot access another patient's appointment.");
            }

            if (User.IsDoctor() && appointment.DoctorId != User.GetDoctorId())
            {
                throw new ForbiddenAccessException(
                    "You cannot access another doctor's appointment.");
            }

            return Ok(appointment);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Patient")]
        public async Task<IActionResult> BookAppointment([FromBody] AppointmentBookingDto dto)
        {
            var loggedInPatientId = User.GetPatientId();

            if (loggedInPatientId == null)
            {
                throw new ForbiddenAccessException(PatientNotLinked);
            }

            var appointmentCreateDto = new AppointmentCreateDto
            {
                PatientId = loggedInPatientId.Value,
                DoctorId = dto.DoctorId,
                ScheduledDate = (DateTime)dto.ScheduledDate!,
                TimeSlot = dto.TimeSlot
            };

            var appointment = await _appointmentService.BookAppointmentAsync(
                appointmentCreateDto);

            return StatusCode(
                StatusCodes.Status201Created,
                appointment);
        }

        [HttpPut("{id:int}/status")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Patient,Doctor,Admin")]
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

            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);

            if (User.IsPatient())
            {
                var loggedInPatientId = User.GetPatientId();

                if (loggedInPatientId == null)
                {
                    throw new ForbiddenAccessException(PatientNotLinked);
                }

                if (appointment.PatientId != loggedInPatientId.Value)
                {
                    throw new ForbiddenAccessException(
                        "You cannot update another patient's appointment.");
                }

                if (appointmentStatus != AppointmentStatus.Cancelled)
                {
                    throw new ForbiddenAccessException(
                        "Patients can only cancel appointments.");
                }
            }

            if (User.IsDoctor())
            {
                var loggedInDoctorId = User.GetDoctorId();

                if (loggedInDoctorId == null)
                {
                    throw new ForbiddenAccessException(DoctorNotLinked);
                }

                if (appointment.DoctorId != loggedInDoctorId.Value)
                {
                    throw new ForbiddenAccessException(
                        "You cannot update another doctor's appointment.");
                }

                if (appointmentStatus == AppointmentStatus.Cancelled)
                {
                    throw new ForbiddenAccessException(
                        "Doctors cannot cancel patient appointments.");
                }
            }

            await _appointmentService.UpdateAppointmentStatusAsync(
                id,
                appointmentStatus,
                cancellationReason);

            return Ok(new
            {
                message = "Appointment status updated successfully."
            });
        }

        [HttpPost("{id:int}/confirm")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Doctor,Admin")]
        public async Task<IActionResult> ConfirmAppointment(int id)
        {
            await EnsureDoctorOwnsAppointmentIfDoctor(id);

            await _appointmentService.UpdateAppointmentStatusAsync(
                id,
                AppointmentStatus.Confirmed);

            return Ok(new
            {
                message = "Appointment confirmed successfully."
            });
        }

        [HttpPost("{id:int}/cancel")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Patient,Admin")]
        public async Task<IActionResult> CancelAppointment(
            int id,
            [FromBody] CancelAppointmentDto dto)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);

            if (User.IsPatient())
            {
                var loggedInPatientId = User.GetPatientId();

                if (loggedInPatientId == null)
                {
                    throw new ForbiddenAccessException(PatientNotLinked);
                }

                if (appointment.PatientId != loggedInPatientId.Value)
                {
                    throw new ForbiddenAccessException(
                        "You cannot cancel another patient's appointment.");
                }
            }

            await _appointmentService.UpdateAppointmentStatusAsync(
                id,
                AppointmentStatus.Cancelled,
                dto.CancellationReason);

            return Ok(new
            {
                message = "Appointment cancelled successfully."
            });
        }

        [HttpPost("{id:int}/complete")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Doctor,Admin")]
        public async Task<IActionResult> CompleteAppointment(int id)
        {
            await EnsureDoctorOwnsAppointmentIfDoctor(id);

            await _appointmentService.UpdateAppointmentStatusAsync(
                id,
                AppointmentStatus.Completed);

            return Ok(new
            {
                message = "Appointment completed successfully."
            });
        }

        [HttpGet("slots")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Patient,Doctor,Admin")]
        public async Task<IActionResult> GetAvailableSlots(
            [FromQuery] int doctorId,
            [FromQuery] DateOnly date)
        {
            var slots = await _appointmentService.GetAvailableSlotsAsync(
                doctorId,
                date);

            return Ok(slots);
        }

        [HttpDelete("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            await _appointmentService.DeleteAppointmentAsync(id);

            return Ok(new
            {
                message = "Appointment deleted successfully."
            });
        }

        private async Task EnsureDoctorOwnsAppointmentIfDoctor(int appointmentId)
        {
            if (!User.IsDoctor())
            {
                return;
            }

            var loggedInDoctorId = User.GetDoctorId();

            if (loggedInDoctorId == null)
            {
                throw new ForbiddenAccessException(DoctorNotLinked);
            }

            var appointment = await _appointmentService.GetAppointmentByIdAsync(
                appointmentId);

            if (appointment.DoctorId != loggedInDoctorId.Value)
            {
                throw new ForbiddenAccessException(
                    "You cannot access another doctor's appointment.");
            }
        }
    }
}