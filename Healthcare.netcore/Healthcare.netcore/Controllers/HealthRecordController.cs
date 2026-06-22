
using HealthAxis.API.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxis.API.Controllers
{
    [ApiController]
    [Route("api/health-records")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class HealthRecordController : ControllerBase
    {
        private readonly IHealthRecordService _service;

        public HealthRecordController(IHealthRecordService service)
        {
            _service = service;
        }

        // ✅ GET /api/health-records/patient/{patientId}
        [Authorize(Roles = "Patient,Doctor")]
        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetByPatient(int patientId)
        {
            var result = await _service.GetByPatientIdAsync(patientId);
            return Ok(result);
        }

        // ✅ GET /api/health-records/{id}
        [Authorize(Roles = "Patient,Doctor")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(result);
        }

        // ✅ POST /api/health-records
        [Authorize(Roles = "Doctor")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateHealthRecordDto dto)
        {
            var result = await _service.AddAsync(dto);
            return Ok(result);
        }
    }
}