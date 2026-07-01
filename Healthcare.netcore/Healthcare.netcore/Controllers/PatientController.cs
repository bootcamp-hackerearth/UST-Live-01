using HealthAxis.Shared.DTOs.Common;
using HealthAxis.Shared.DTOs.Patient;
using HealthAxis.API.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthAxis.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/patients")]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _service;

        public PatientController(IPatientService service)
        {
            _service = service;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams paginationParams)
        {
            var result = await _service.GetPagedAsync(paginationParams);
            return Ok(result);
        }

        [Authorize(Roles = "Doctor")]
        [HttpGet("doctor")]
        public async Task<IActionResult> GetDoctorPatients([FromQuery] PaginationParams paginationParams)
        {
            var doctorUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(doctorUserId))
            {
                return Unauthorized();
            }

            var result = await _service.GetDoctorPatientsAsync(doctorUserId, paginationParams);
            return Ok(result);
        }
        [Authorize(Roles = "Patient")]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentPatient()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var patient = await _service.GetByUserIdAsync(userId);

            return Ok(patient);
        }

        [Authorize(Roles = "Patient")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var isOwner = await _service.IsPatientOwnerAsync(id, userId);

            if (!isOwner)
            {
                return Forbid();
            }

            var patient = await _service.GetByIdAsync(id);
            return Ok(patient);
        }

        // ✅ Admin can create patient
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(CreatePatientDto dto)
        {
            var result = await _service.AddAsync(dto);
            return Ok(result);
        }

        // ✅ Patient can update own profile, Admin can update any patient
        [Authorize(Roles = "Patient,Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdatePatientDto dto)
        {
            if (User.IsInRole("Admin"))
            {
                var adminResult = await _service.UpdateAsync(id, dto);
                return Ok(adminResult);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var isOwner = await _service.IsPatientOwnerAsync(id, userId);

            if (!isOwner)
            {
                return Forbid();
            }

            var result = await _service.UpdateAsync(id, dto);
            return Ok(result);
        }

        [Authorize(Roles = "Patient")]
        [HttpGet("{id}/health-records")]
        public async Task<IActionResult> GetHealthRecords(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var isOwner = await _service.IsPatientOwnerAsync(id, userId);

            if (!isOwner)
            {
                return Forbid();
            }

            var records = await _service.GetHealthRecordsAsync(id);
            return Ok(records);
        }
    }
}