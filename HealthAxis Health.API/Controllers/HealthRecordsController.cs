using HealthAxisHealth.Shared.DTOs.HealthRecordDtos;
using HealthAxisHealth.API.Helpers;
using HealthAxisHealth.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace HealthAxisHealth.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    [ExcludeFromCodeCoverage]
    public class HealthRecordsController :
        ControllerBase
    {
        #region Fields

        private readonly IHealthRecordService
            _healthRecordService;

        #endregion

        #region Constructor

        public HealthRecordsController(
            IHealthRecordService healthRecordService)
        {
            _healthRecordService =
                healthRecordService;
        }

        #endregion

        #region Endpoints

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult>
            GetHealthRecords(
                [FromQuery]
                PaginationParams pagination)
        {
            var healthRecords =
                await _healthRecordService
                    .GetPagedAsync(
                        pagination);

            return Ok(healthRecords);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult>
            GetById(
                int id)
        {
            var healthRecord =
                await _healthRecordService
                    .GetByIdAsync(id);

            return Ok(healthRecord);
        }

        [HttpGet("patient/{patientId:int}")]
        public async Task<IActionResult>
            GetByPatient(
                int patientId)
        {
            var healthRecords =
                await _healthRecordService
                    .GetByPatientIdAsync(
                        patientId);

            return Ok(healthRecords);
        }

        [HttpGet("doctor/{doctorId:int}")]
        [Authorize(Roles = "Doctor,Admin")]
        public async Task<IActionResult>
            GetByDoctor(
                int doctorId)
        {
            var healthRecords =
                await _healthRecordService
                    .GetByDoctorIdAsync(
                        doctorId);

            return Ok(healthRecords);
        }

        [HttpPost]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult>
            Create(
                CreateHealthRecordDto dto)
        {
            int recordId =
                await _healthRecordService
                    .CreateAsync(dto);

            return Ok(
                new
                {
                    RecordId = recordId,
                    Message =
                        "Health record created successfully."
                });
        }

        #endregion
    }
}
