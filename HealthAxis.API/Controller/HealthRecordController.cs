using HealthAxis.API.Data;
using HealthAxis.API.Services;
using HealthAxis.API.Services.Interfaces;
using HealthAxis.Shared.DTO.HealthRecordDtos;
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
        private const string InvalidToken = "Invalid token";
        private const string DoctorProfileNotFound = "Doctor profile not found";


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
            _context = context;
            _healthRecordService = healthRecordService;
            _patientService = patientService;
            _doctorService = doctorService;
        }

        [HttpGet("patient/{patientId}")]
        [Authorize(Roles = "Patient,Doctor")]
        public async Task<IActionResult> GetByPatientId(int patientId)
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
                var isAllowed = await IsLoggedInPatientAsync(
                    userId,
                    patientId);

                if (!isAllowed)
                {
                    return StatusCode(403, new
                    {
                        message = "You are not allowed to access another patient's health records"
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
                        message = DoctorProfileNotFound
                    });
                }

                var hasAccess = await DoctorHasAccessToPatientAsync(
                    doctor.DoctorId,
                    patientId);

                if (!hasAccess)
                {
                    return StatusCode(403, new
                    {
                        message = "You are not allowed to access this patient's health records"
                    });
                }
            }

            var records = await _healthRecordService.GetByPatientIdAsync(patientId);

            return Ok(records);
        }

        [HttpGet("{id}")]
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

            var record = await _healthRecordService.GetByIdAsync(id);

            if (record == null)
            {
                return NotFound(new
                {
                    message = "Health record not found"
                });
            }

            if (User.IsInRole("Patient"))
            {
                var isAllowed = await IsLoggedInPatientAsync(
                    userId,
                    record.PatientId);

                if (!isAllowed)
                {
                    return StatusCode(403, new
                    {
                        message = "You are not allowed to access another patient's health record"
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
                        message = DoctorProfileNotFound
                    });
                }

                var hasAccess = await DoctorHasAccessToPatientAsync(
                    doctor.DoctorId,
                    record.PatientId);

                if (!hasAccess)
                {
                    return StatusCode(403, new
                    {
                        message = "You are not allowed to access this health record"
                    });
                }
            }

            return Ok(record);
        }

        [HttpPost]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> CreateHealthRecord(
            [FromBody] CreateHealthRecordDto healthRecordDto)
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

            var doctor = await _doctorService.GetByUserIdAsync(userId);

            if (doctor == null)
            {
                return NotFound(new
                {
                    message = DoctorProfileNotFound
                });
            }

            var record = await _healthRecordService.AddAsync(
                healthRecordDto,
                doctor.DoctorId);

            return Ok(record);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> UpdateHealthRecord(
            int id,
            [FromBody] UpdateHealthRecordDto healthRecordDto)
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

            var doctor = await _doctorService.GetByUserIdAsync(userId);

            if (doctor == null)
            {
                return NotFound(new
                {
                    message = DoctorProfileNotFound
                });
            }

            var updatedRecord = await _healthRecordService.UpdateAsync(
                id,
                healthRecordDto,
                doctor.DoctorId);

            if (updatedRecord == null)
            {
                return NotFound(new
                {
                    message = "Health record not found"
                });
            }

            return Ok(updatedRecord);
        }

        private async Task<bool> IsLoggedInPatientAsync(
            string userId,
            int patientId)
        {
            var patient = await _patientService.GetByUserIdAsync(userId);

            return patient != null && patient.PatientId == patientId;
        }

        private async Task<bool> DoctorHasAccessToPatientAsync(
            int doctorId,
            int patientId)
        {
            var hasAppointment = await _context.Appointments
                .AsNoTracking()
                .AnyAsync(appointment =>
                    appointment.DoctorId == doctorId &&
                    appointment.PatientId == patientId);

            if (hasAppointment)
            {
                return true;
            }

            return await _context.HealthRecords
                .AsNoTracking()
                .AnyAsync(record =>
                    record.DoctorId == doctorId &&
                    record.PatientId == patientId);
        }

        private string? GetLoggedInUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? User.FindFirstValue("sub")
                ?? User.FindFirstValue("nameid");
        }
    }
}