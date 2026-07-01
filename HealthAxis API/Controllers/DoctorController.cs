using HealthAxis.API.DTOs.Doctors;
using HealthAxis.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxis.API.Controllers
{
    [Route("api/doctors")]
    [ApiController]
    [Authorize]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorService _doctorService;
        private readonly IPatientService _patientService;
        private readonly IAppointmentService _appointmentService;
        private readonly IHealthRecordService _healthRecordService;

        public DoctorsController(
            IDoctorService doctorService,
            IPatientService patientService,
            IAppointmentService appointmentService,
            IHealthRecordService healthRecordService)
        {
            _doctorService = doctorService;
            _patientService = patientService;
            _appointmentService = appointmentService;
            _healthRecordService = healthRecordService;
        }

        [HttpGet]
        [Authorize(Roles = "Patient,Doctor,Admin")]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var doctors = await _doctorService.GetAllAsync(ct);
            return Ok(doctors);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Patient,Doctor,Admin")]
        public async Task<IActionResult> GetById(
            int id,
            CancellationToken ct)
        {
            var doctor = await _doctorService.GetByIdAsync(id, ct);

            if (doctor == null)
            {
                return NotFound(new
                {
                    message = "Doctor not found."
                });
            }

            return Ok(doctor);
        }

        [HttpGet("{id:int}/availability")]
        [Authorize(Roles = "Patient,Admin")]
        public async Task<IActionResult> GetAvailability(
            int id,
            [FromQuery] DateTime date,
            CancellationToken ct)
        {
            if (date == default)
            {
                return BadRequest(new
                {
                    message = "Appointment date is required."
                });
            }

            if (date.Date < DateTime.Today)
            {
                return BadRequest(new
                {
                    message = "Past dates are not allowed."
                });
            }

            DateTime maxAllowedDate =
            DateTime.Today.AddMonths(6);

            if (date.Date > maxAllowedDate)
            {
                return BadRequest(new
                {
                    message = "Appointments can only be booked up to 6 months in advance."
                });
            }

            var availableSlots =
                await _doctorService.GetAvailableSlotsAsync(
                    id,
                    date.Date,
                    ct);

            return Ok(availableSlots);
        }

        [HttpPatch("{id:int}/active-status")]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<IActionResult> UpdateActiveStatus(
            int id,
            [FromBody] DoctorActiveStatusUpdateDto request,
            CancellationToken ct)
        {
            bool isAdmin = User.IsInRole("Admin");

            if (!isAdmin)
            {
                int loggedInDoctorId = GetReferenceIdFromToken();

                if (loggedInDoctorId != id)
                {
                    return Forbid();
                }
            }

            var doctor = await _doctorService.UpdateActiveStatusAsync(
                id,
                request.IsActive,
                ct);

            if (doctor == null)
            {
                return NotFound(new
                {
                    message = "Doctor not found."
                });
            }

            return Ok(doctor);
        }

        [HttpGet("patients/{patientId:int}")]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<IActionResult> GetPatientForDoctor(
            int patientId,
            CancellationToken ct)
        {
            bool isAdmin = User.IsInRole("Admin");

            if (!isAdmin)
            {
                int doctorId = GetReferenceIdFromToken();

                var doctorAppointments =
                    await _appointmentService.GetAppointmentsByDoctorIdAsync(
                        doctorId,
                        ct);

                bool hasAppointmentWithPatient =
                    doctorAppointments.Any(a => a.PatientId == patientId);

                if (!hasAppointmentWithPatient)
                {
                    return Forbid();
                }
            }

            var patient =
                await _patientService.GetByIdAsync(
                    patientId,
                    ct);

            if (patient == null)
            {
                return NotFound(new
                {
                    message = "Patient not found."
                });
            }

            return Ok(patient);
        }

        [HttpGet("patients/{patientId:int}/health-records")]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<IActionResult> GetPatientHealthRecordsForDoctor(
            int patientId,
            CancellationToken ct)
        {
            bool isAdmin = User.IsInRole("Admin");

            if (!isAdmin)
            {
                int doctorId = GetReferenceIdFromToken();

                var doctorAppointments =
                    await _appointmentService.GetAppointmentsByDoctorIdAsync(
                        doctorId,
                        ct);

                bool hasAppointmentWithPatient =
                    doctorAppointments.Any(a => a.PatientId == patientId);

                if (!hasAppointmentWithPatient)
                {
                    return Forbid();
                }
            }

            var records =
                await _healthRecordService.GetByPatientIdAsync(
                    patientId,
                    ct);

            return Ok(records);
        }

        private int GetReferenceIdFromToken()
        {
            string? referenceIdClaim =
                User.FindFirst("ReferenceId")?.Value;

            if (string.IsNullOrWhiteSpace(referenceIdClaim) ||
                !int.TryParse(referenceIdClaim, out int referenceId))
            {
                throw new UnauthorizedAccessException(
                    "Reference Id was not found in token.");
            }

            return referenceId;
        }
    }
}
