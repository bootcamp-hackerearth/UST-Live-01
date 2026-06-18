using HealthAxis.API.DTOs;
using HealthAxis.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace HealthAxis.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Patient")]
    [ApiController]
    [Route("api/patients")]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _service;

        public PatientController(IPatientService service)
        {
            _service = service;
        }

        // ✅ GET all patients
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        // ✅ GET patient by id
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var patient = await _service.GetByIdAsync(id);
            return Ok(patient);
        }

        // ✅ UPDATE patient
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdatePatientDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            return Ok(result);
        }

        // ✅ GET health records of patient
        [HttpGet("{id}/health-records")]
        public async Task<IActionResult> GetHealthRecords(int id)
        {
            var records = await _service.GetHealthRecordsAsync(id);
            return Ok(records);
        }

        // ✅ SEARCH patient by name
        [HttpGet("search")]
        public async Task<IActionResult> SearchByName(string name)
        {
            var result = await _service.SearchByNameAsync(name);
            return Ok(result);
        }

        // ✅ GET by email
        [HttpGet("by-email")]
        public async Task<IActionResult> GetByEmail(string email)
        {
            var result = await _service.GetByEmailAsync(email);
            return Ok(result);
        }

        // ✅ GET by phone
        [HttpGet("by-phone")]
        public async Task<IActionResult> GetByPhone(string phone)
        {
            var result = await _service.GetByPhoneAsync(phone);
            return Ok(result);
        }
    }
}