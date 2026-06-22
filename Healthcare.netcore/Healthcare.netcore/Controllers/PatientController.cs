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

        // ✅ Admin can see all patients with pagination
        // GET /api/patients?pageNumber=1&pageSize=10
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationParams paginationParams)
        {
            var result = await _service.GetPagedAsync(paginationParams);
            return Ok(result);
        }

        // ✅ Doctor can see only patients who have appointments with that doctor
        // GET /api/patients/doctor?pageNumber=1&pageSize=10
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

        // ✅ Patient can view only own profile
        // GET /api/patients/{id}
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

        // ✅ Patient can update only own profile
        // PUT /api/patients/{id}
        [Authorize(Roles = "Patient")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdatePatientDto dto)
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

            var result = await _service.UpdateAsync(id, dto);
            return Ok(result);
        }

        // ✅ Patient can view only own health records
        // GET /api/patients/{id}/health-records
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