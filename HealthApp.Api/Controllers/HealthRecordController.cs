using HealthApp.Api.Dtos;
using HealthApp.Api.Exceptions;
using HealthApp.Api.Extensions;
using HealthApp.Api.Services.Interfaces;
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

        public HealthRecordsController(
            IHealthRecordService healthRecordService,
            IAppointmentService appointmentService)
        {
            _healthRecordService = healthRecordService;
            _appointmentService = appointmentService;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> GetAllHealthRecords()
        {
            var records = await _healthRecordService.GetAllAsync();

            return Ok(records);
        }

        [HttpGet("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Patient,Doctor,Admin")]
        public async Task<IActionResult> GetHealthRecordById(int id)
        {
            var record = await _healthRecordService.GetByIdAsync(id);

            if (User.IsPatient() && record.PatientId != User.GetPatientId())
            {
                throw new ForbiddenAccessException(
                    "You cannot access another patient's health record.");
            }

            if (User.IsDoctor() && record.DoctorId != User.GetDoctorId())
            {
                throw new ForbiddenAccessException(
                    "You cannot access another doctor's health record.");
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
                throw new ForbiddenAccessException(
                    "Doctor profile is not linked to this user.");
            }

            dto.DoctorId = loggedInDoctorId.Value;

            await _healthRecordService.AddAsync(dto);

            return StatusCode(
                StatusCodes.Status201Created,
                new
                {
                    message = "Health record added successfully."
                });
        }

        [HttpGet("patient/{patientId:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Patient,Doctor,Admin")]
        public async Task<IActionResult> GetHealthRecordsByPatient(int patientId)
        {
            if (User.IsPatient() && User.GetPatientId() != patientId)
            {
                throw new ForbiddenAccessException(
                    "You cannot access another patient's health records.");
            }

            var records = await _healthRecordService.GetPatientHistoryAsync(
                patientId);

            return Ok(records);
        }

        [HttpGet("~/api/patients/{id:int}/health-records")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Doctor,Admin")]
        public async Task<IActionResult> GetPatientHealthRecords(int id)
        {
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
                throw new ForbiddenAccessException(
                    "Patient profile is not linked to this user.");
            }

            var records = await _healthRecordService.GetPatientHistoryAsync(
                patientId.Value);

            return Ok(records);
        }

        [HttpGet("~/api/appointments/{id:int}/healthrecord")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Doctor,Admin")]
        public async Task<IActionResult> HealthRecordExists(int id)
        {
            await EnsureDoctorOwnsAppointmentIfDoctor(id);

            var exists = await _healthRecordService.ExistsByAppointmentIdAsync(id);

            return Ok(exists);
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
                throw new ForbiddenAccessException(
                    "Doctor profile is not linked to this user.");
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