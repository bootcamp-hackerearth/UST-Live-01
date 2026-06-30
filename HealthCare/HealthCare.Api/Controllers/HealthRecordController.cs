using Healthcare.Shared.DTOs.HealthRecord;
using Healthcare.Shared.DTOs.Patient;
using HealthCare.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HealthCare.Api.Controllers
{
    [Route("api/healthrecords")]
    [ApiController]
    public class HealthRecordController : ControllerBase
    {

        private readonly IHealthRecordService _healthRecordService;

        public HealthRecordController(IHealthRecordService healthRecordService)
        {
            _healthRecordService = healthRecordService;
        }

        //Create
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Doctor")]
        public async Task<IActionResult> Add([FromBody] CreateHealthRecordDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var doctorId = GetDoctorIdFromClaims();

            await _healthRecordService.AddAsync(dto, doctorId);

            return Ok(new { message = "Health record created successfully" });
        }

        //  GET ALL Health Records
        [HttpGet]
        public async Task<IActionResult> GetAll(HealthRecordFilter filter)
        {
            var records = await _healthRecordService.GetAllAsync(filter);
            return Ok(records);
        }

        //  GET Health Record By ID
        [HttpGet("{id:int}")]

        public async Task<IActionResult> GetById(int id)
        {
            var record = await _healthRecordService.GetByIdAsync(id);

            if (record == null)
                return NotFound();

            return Ok(record);
        }

        //  UPDATE Health Record
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateHealthRecordDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _healthRecordService.UpdateAsync(id, dto);

            return Ok(new { message = "Health record updated successfully" });
        }


        [HttpGet("my-records")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> GetHealthRecordByPatient()
        {
            var patientId = GetPatientIdFromClaims();
            var result = await _healthRecordService.GetHealthRecordByPatient(patientId);
            return Ok(result);
        }

        private int GetPatientIdFromClaims()
        {
            var claim = User.FindFirst("PatientId")
                ?? throw new InvalidOperationException("PatientId claim not found in token.");

            return int.Parse(claim.Value);
        }

        private int GetDoctorIdFromClaims()
        {
            var claim = User.FindFirst("DoctorId")
                ?? throw new InvalidOperationException("DoctorId claim not found in token.");

            return int.Parse(claim.Value);
        }


    }
}
