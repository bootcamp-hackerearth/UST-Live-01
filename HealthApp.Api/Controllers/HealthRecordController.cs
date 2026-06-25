using HealthApp.Api.Exceptions;
using HealthApp.Api.Extensions;
using HealthApp.Api.Services.Interfaces;
using HealthApp.Shared.Dtos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthApp.Api.Controllers
{
    [ApiController]
    [Route("api/health-records")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class HealthRecordsController : ControllerBase
    {
        private readonly IHealthRecordService _healthRecordService;
        private readonly IAppointmentService _appointmentService;

        private const string DoctorProfileNotLinkedMessage =
            "Doctor profile is not linked to this user.";

        private const string PatientProfileNotLinkedMessage =
            "Patient profile is not linked to this user.";

        private const string OnlyOwnHealthRecordsMessage =
            "You can view only your own health records.";

        private const string OnlyAssignedAppointmentsMessage =
            "You can access only appointments assigned to you.";

        private const string OnlyTreatingPatientsMessage =
            "You can view health records only for patients you treat.";

        public HealthRecordsController(
            IHealthRecordService healthRecordService,
            IAppointmentService appointmentService)
        {
            _healthRecordService = healthRecordService;
            _appointmentService = appointmentService;
        }

        [HttpGet("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Patient,Doctor")]
        public async Task<IActionResult> GetHealthRecordById(int id)
        {
            var record = await _healthRecordService.GetByIdAsync(id);

            if (User.IsPatient())
            {
                var loggedInPatientId = User.GetPatientId();

                if (loggedInPatientId == null)
                {
                    throw new ForbiddenAccessException(PatientProfileNotLinkedMessage);
                }

                if (record.PatientId != loggedInPatientId.Value)
                {
                    throw new ForbiddenAccessException(OnlyOwnHealthRecordsMessage);
                }
            }

            if (User.IsDoctor())
            {
                var loggedInDoctorId = User.GetDoctorId();

                if (loggedInDoctorId == null)
                {
                    throw new ForbiddenAccessException(DoctorProfileNotLinkedMessage);
                }

                await EnsureDoctorTreatsPatientAsync(
                    loggedInDoctorId.Value,
                    record.PatientId);
            }

            return Ok(record);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Doctor")]
        public async Task<IActionResult> AddHealthRecord([FromBody] HealthRecordCreateDto dto)
        {
            var loggedInDoctorId = User.GetDoctorId();

            if (loggedInDoctorId == null)
            {
                throw new ForbiddenAccessException(DoctorProfileNotLinkedMessage);
            }

            dto.DoctorId = loggedInDoctorId.Value;

            if (dto.AppointmentId.HasValue)
            {
                await EnsureDoctorOwnsAppointmentAsync(
                    loggedInDoctorId.Value,
                    dto.AppointmentId.Value);
            }
            else
            {
                await EnsureDoctorTreatsPatientAsync(
                    loggedInDoctorId.Value,
                    dto.PatientId);
            }

            await _healthRecordService.AddAsync(dto);

            return StatusCode(StatusCodes.Status201Created, new
            {
                message = "Health record added successfully."
            });
        }

        [HttpGet("patient/{patientId:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Patient,Doctor")]
        public async Task<IActionResult> GetHealthRecordsByPatient(int patientId)
        {
            if (User.IsPatient())
            {
                var loggedInPatientId = User.GetPatientId();

                if (loggedInPatientId == null)
                {
                    throw new ForbiddenAccessException(PatientProfileNotLinkedMessage);
                }

                if (loggedInPatientId.Value != patientId)
                {
                    throw new ForbiddenAccessException(OnlyOwnHealthRecordsMessage);
                }
            }

            if (User.IsDoctor())
            {
                var loggedInDoctorId = User.GetDoctorId();

                if (loggedInDoctorId == null)
                {
                    throw new ForbiddenAccessException(DoctorProfileNotLinkedMessage);
                }

                await EnsureDoctorTreatsPatientAsync(
                    loggedInDoctorId.Value,
                    patientId);
            }

            var records = await _healthRecordService.GetPatientHistoryAsync(patientId);

            return Ok(records);
        }

        [HttpGet("~/api/patients/{id:int}/health-records")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Doctor")]
        public async Task<IActionResult> GetPatientHealthRecords(int id)
        {
            var loggedInDoctorId = User.GetDoctorId();

            if (loggedInDoctorId == null)
            {
                throw new ForbiddenAccessException(DoctorProfileNotLinkedMessage);
            }

            await EnsureDoctorTreatsPatientAsync(
                loggedInDoctorId.Value,
                id);

            var records = await _healthRecordService.GetPatientHistoryAsync(id);

            return Ok(records);
        }

        [HttpGet("~/api/patients/profile/health-records")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Patient")]
        public async Task<IActionResult> GetMyHealthRecords()
        {
            var patientId = User.GetPatientId();

            if (patientId == null)
            {
                throw new ForbiddenAccessException(PatientProfileNotLinkedMessage);
            }

            var records = await _healthRecordService.GetPatientHistoryAsync(patientId.Value);

            return Ok(records);
        }

        [HttpGet("~/api/appointments/{id:int}/healthrecord")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Doctor")]
        public async Task<IActionResult> HealthRecordExists(int id)
        {
            var loggedInDoctorId = User.GetDoctorId();

            if (loggedInDoctorId == null)
            {
                throw new ForbiddenAccessException(DoctorProfileNotLinkedMessage);
            }

            await EnsureDoctorOwnsAppointmentAsync(
                loggedInDoctorId.Value,
                id);

            var exists = await _healthRecordService.ExistsByAppointmentIdAsync(id);

            return Ok(exists);
        }

        private async Task EnsureDoctorOwnsAppointmentAsync(int doctorId, int appointmentId)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(appointmentId);

            if (appointment.DoctorId != doctorId)
            {
                throw new ForbiddenAccessException(OnlyAssignedAppointmentsMessage);
            }
        }

        private async Task EnsureDoctorTreatsPatientAsync(int doctorId, int patientId)
        {
            var appointments = await _appointmentService.GetAppointmentsAsync(
                new AppointmentFilterDto
                {
                    DoctorId = doctorId,
                    PatientId = patientId,
                    OnlyUpcoming = false
                });

            var hasTreatmentRelationship = appointments.Items.Any(appointment =>
                appointment.Status == "Confirmed" ||
                appointment.Status == "Completed");

            if (!hasTreatmentRelationship)
            {
                throw new ForbiddenAccessException(OnlyTreatingPatientsMessage);
            }
        }
    }
}