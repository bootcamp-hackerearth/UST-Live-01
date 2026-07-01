using HealthAxis.Shared.DTO.HealthRecordDtos;
using HealthAxis.Shared.Enums;
using HealthAxis.API.Services;
using HealthAxis.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthAxis.API.Controller
{
    [Route("api/health-records")]
    [ApiController]
    public class HealthRecordController : ControllerBase
    {
        private readonly IHealthRecordService _healthRecordService;
        private readonly IPatientService _patientService;
        private readonly IDoctorService _doctorService;
        private readonly IAppointmentService _appointmentService;

        public HealthRecordController(
            IHealthRecordService healthRecordService,
            IPatientService patientService,
            IDoctorService doctorService,
            IAppointmentService appointmentService)
        {
            _healthRecordService = healthRecordService;
            _patientService = patientService;
            _doctorService = doctorService;
            _appointmentService = appointmentService;
        }

        private string? GetLoggedInUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
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

                if (patient.PatientId != patientId)
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
                        message = "Doctor profile not found"
                    });
                }

                var allowedPatient = await _patientService.GetPatientForDoctorAsync(
                    doctor.DoctorId,
                    patientId);

                if (allowedPatient == null)
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
                    message = "Invalid token"
                });
            }

            var doctor = await _doctorService.GetByUserIdAsync(userId);

            if (doctor == null)
            {
                return NotFound(new
                {
                    message = "Doctor profile not found"
                });
            }

            var appointment = await _appointmentService.GetByIdAsync(
                healthRecordDto.AppointmentId);

            if (appointment == null)
            {
                return NotFound(new
                {
                    message = "Appointment not found"
                });
            }

            if (appointment.DoctorId != doctor.DoctorId)
            {
                return StatusCode(403, new
                {
                    message = "You cannot add health record for another doctor's appointment"
                });
            }

            if (appointment.Status != AppointmentStatus.Confirmed)
            {
                return BadRequest(new
                {
                    message = "Health record can be added only for confirmed appointments"
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
                    message = "Invalid token"
                });
            }

            var doctor = await _doctorService.GetByUserIdAsync(userId);

            if (doctor == null)
            {
                return NotFound(new
                {
                    message = "Doctor profile not found"
                });
            }

            var updatedRecord = await _healthRecordService.UpdateAsync(
                id,
                healthRecordDto,
                doctor.DoctorId);

            return Ok(updatedRecord);
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
                    message = "Invalid token"
                });
            }

            var record = await _healthRecordService.GetByIdAsync(id);

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

                if (record.PatientId != patient.PatientId)
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
                        message = "Doctor profile not found"
                    });
                }

                var allowedPatient = await _patientService.GetPatientForDoctorAsync(
                    doctor.DoctorId,
                    record.PatientId);

                if (allowedPatient == null)
                {
                    return StatusCode(403, new
                    {
                        message = "You are not allowed to access this health record"
                    });
                }
            }

            return Ok(record);
        }
    }
}