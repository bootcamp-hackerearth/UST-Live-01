using HealthAxis.API.Data;
using HealthAxis.API.Services;
using HealthAxis.API.Services.Interfaces;
using HealthAxis.Shared.DTO.HealthRecordDtos;
using HealthAxis.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HealthAxis.API.Controller
{
    [Route("api/health-records")]
    [ApiController]
    public class HealthRecordController : ControllerBase
    {
        private const string InvalidToken =
            "Invalid token";

        private const string DoctorProfileNotFound =
            "Doctor profile not found";

        private const string HealthRecordNotFound =
            "Health record not found";

        private const string PatientRecordsAccessDenied =
            "You are not allowed to access this patient's health records";

        private const string PatientRecordAccessDenied =
            "You are not allowed to access this health record";

        private const string OtherPatientRecordsAccessDenied =
            "You are not allowed to access another patient's health records";

        private const string OtherPatientRecordAccessDenied =
            "You are not allowed to access another patient's health record";

        private const string AppointmentNotFound =
            "Appointment not found";

        private const string AppointmentHistoryAccessDenied =
            "You are not allowed to access patient history for this appointment";

        private const string ConfirmedAppointmentRequired =
            "Patient history is available only while the appointment is confirmed";

        private readonly ApplicationDbContext _context;
        private readonly IHealthRecordService _healthRecordService;
        private readonly IPatientService _patientService;
        private readonly IDoctorService _doctorService;

        public HealthRecordController(
            ApplicationDbContext context,
            IHealthRecordService healthRecordService,
            IPatientService patientService,
            IDoctorService doctorService)
        {
            ArgumentNullException.ThrowIfNull(context);
            ArgumentNullException.ThrowIfNull(healthRecordService);
            ArgumentNullException.ThrowIfNull(patientService);
            ArgumentNullException.ThrowIfNull(doctorService);

            _context = context;
            _healthRecordService = healthRecordService;
            _patientService = patientService;
            _doctorService = doctorService;
        }

        [HttpGet("patient/{patientId:int}")]
        [Authorize(Roles = "Patient,Doctor")]
        public async Task<IActionResult> GetByPatientId(
            int patientId)
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
                var isAllowed =
                    await IsLoggedInPatientAsync(
                        userId,
                        patientId);

                if (!isAllowed)
                {
                    return StatusCode(
                        StatusCodes.Status403Forbidden,
                        new
                        {
                            message =
                                OtherPatientRecordsAccessDenied
                        });
                }
            }

            if (User.IsInRole("Doctor"))
            {
                var doctor =
                    await _doctorService.GetByUserIdAsync(
                        userId);

                if (doctor == null)
                {
                    return NotFound(new
                    {
                        message = DoctorProfileNotFound
                    });
                }

                var hasAccess =
                    await DoctorHasConfirmedAppointmentAsync(
                        doctor.DoctorId,
                        patientId);

                if (!hasAccess)
                {
                    return StatusCode(
                        StatusCodes.Status403Forbidden,
                        new
                        {
                            message =
                                PatientRecordsAccessDenied
                        });
                }
            }

            var records =
                await _healthRecordService
                    .GetByPatientIdAsync(patientId);

            return Ok(records);
        }

        [HttpGet(
            "appointment/{appointmentId:int}/history")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult>
            GetPatientHistoryByAppointmentId(
                int appointmentId)
        {
            var userId = GetLoggedInUserId();

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized(new
                {
                    message = InvalidToken
                });
            }

            var doctor =
                await _doctorService.GetByUserIdAsync(userId);

            if (doctor == null)
            {
                return NotFound(new
                {
                    message = DoctorProfileNotFound
                });
            }

            var appointment =
                await _context.Appointments
                    .AsNoTracking()
                    .Where(item =>
                        item.AppointmentId ==
                        appointmentId)
                    .Select(item => new
                    {
                        item.PatientId,
                        item.DoctorId,
                        item.Status
                    })
                    .FirstOrDefaultAsync();

            if (appointment == null)
            {
                return NotFound(new
                {
                    message = AppointmentNotFound
                });
            }

            if (appointment.DoctorId != doctor.DoctorId)
            {
                return StatusCode(
                    StatusCodes.Status403Forbidden,
                    new
                    {
                        message =
                            AppointmentHistoryAccessDenied
                    });
            }

            if (appointment.Status !=
                AppointmentStatus.Confirmed)
            {
                return StatusCode(
                    StatusCodes.Status403Forbidden,
                    new
                    {
                        message =
                            ConfirmedAppointmentRequired
                    });
            }

            var records =
                await _healthRecordService
                    .GetByPatientIdAsync(
                        appointment.PatientId);

            return Ok(records);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Patient,Doctor")]
        public async Task<IActionResult> GetById(int id)
        {
            var userId = GetLoggedInUserId();

            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized(new
                {
                    message = InvalidToken
                });
            }

            var record =
                await _healthRecordService.GetByIdAsync(id);

            if (record == null)
            {
                return NotFound(new
                {
                    message = HealthRecordNotFound
                });
            }

            if (User.IsInRole("Patient"))
            {
                var isAllowed =
                    await IsLoggedInPatientAsync(
                        userId,
                        record.PatientId);

                if (!isAllowed)
                {
                    return StatusCode(
                        StatusCodes.Status403Forbidden,
                        new
                        {
                            message =
                                OtherPatientRecordAccessDenied
                        });
                }
            }

            if (User.IsInRole("Doctor"))
            {
                var doctor =
                    await _doctorService.GetByUserIdAsync(
                        userId);

                if (doctor == null)
                {
                    return NotFound(new
                    {
                        message = DoctorProfileNotFound
                    });
                }

                var hasConfirmedAppointment =
                    await DoctorHasConfirmedAppointmentAsync(
                        doctor.DoctorId,
                        record.PatientId);

                var isRecordOwner =
                    record.DoctorId == doctor.DoctorId;

                if (!hasConfirmedAppointment &&
                    !isRecordOwner)
                {
                    return StatusCode(
                        StatusCodes.Status403Forbidden,
                        new
                        {
                            message =
                                PatientRecordAccessDenied
                        });
                }
            }

            return Ok(record);
        }

        [HttpPost]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult>
            CreateHealthRecord(
                [FromBody]
                CreateHealthRecordDto healthRecordDto)
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

            var doctor =
                await _doctorService.GetByUserIdAsync(
                    userId);

            if (doctor == null)
            {
                return NotFound(new
                {
                    message = DoctorProfileNotFound
                });
            }

            var record =
                await _healthRecordService.AddAsync(
                    healthRecordDto,
                    doctor.DoctorId);

            return Ok(record);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult>
            UpdateHealthRecord(
                int id,
                [FromBody]
                UpdateHealthRecordDto healthRecordDto)
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

            var doctor =
                await _doctorService.GetByUserIdAsync(
                    userId);

            if (doctor == null)
            {
                return NotFound(new
                {
                    message = DoctorProfileNotFound
                });
            }

            var updatedRecord =
                await _healthRecordService.UpdateAsync(
                    id,
                    healthRecordDto,
                    doctor.DoctorId);

            if (updatedRecord == null)
            {
                return NotFound(new
                {
                    message = HealthRecordNotFound
                });
            }

            return Ok(updatedRecord);
        }

        private async Task<bool>
            IsLoggedInPatientAsync(
                string userId,
                int patientId)
        {
            var patient =
                await _patientService.GetByUserIdAsync(
                    userId);

            return patient != null &&
                   patient.PatientId == patientId;
        }

        private Task<bool>
            DoctorHasConfirmedAppointmentAsync(
                int doctorId,
                int patientId)
        {
            return _context.Appointments
                .AsNoTracking()
                .AnyAsync(appointment =>
                    appointment.DoctorId == doctorId &&
                    appointment.PatientId == patientId &&
                    appointment.Status ==
                    AppointmentStatus.Confirmed);
        }

        private string? GetLoggedInUserId()
        {
            return User.FindFirstValue(
                       ClaimTypes.NameIdentifier)
                   ?? User.FindFirstValue("sub")
                   ?? User.FindFirstValue("nameid");
        }
    }
}