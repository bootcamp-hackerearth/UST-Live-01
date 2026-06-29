using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using S3_HealthAxis.Shared.DTOs.Patient;
using S3_HealthAxisApi.Services.Interface;

namespace S3_HealthAxisApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;
        private readonly IHealthRecordService _healthRecordService;

        public PatientsController(
            IPatientService patientService,
            IHealthRecordService healthRecordService)
        {
            _patientService = patientService;
            _healthRecordService = healthRecordService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var patients = await _patientService.GetAllAsync();

            return Ok(patients);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Doctor,Patient")]
        public async Task<IActionResult> GetById(int id)
        {
            if (User.IsInRole("Patient"))
            {
                var patientIdFromToken = GetPatientReferenceIdFromToken();

                if (!patientIdFromToken.HasValue ||
                    patientIdFromToken.Value != id)
                {
                    return Forbid();
                }
            }

            var patient = await _patientService.GetByIdAsync(id);

            if (patient == null)
            {
                return NotFound($"Patient with Id {id} not found.");
            }

            return Ok(patient);
        }

        [HttpGet("{id:int}/health-records")]
        [Authorize(Roles = "Admin,Doctor,Patient")]
        public async Task<IActionResult> GetHealthRecords(int id)
        {
            if (User.IsInRole("Patient"))
            {
                var patientIdFromToken = GetPatientReferenceIdFromToken();

                if (!patientIdFromToken.HasValue ||
                    patientIdFromToken.Value != id)
                {
                    return Forbid();
                }
            }

            try
            {
                var records =
                    await _healthRecordService.GetByPatientIdAsync(id);

                return Ok(records);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("search")]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> Search(
            [FromQuery] string name)
        {
            var patients =
                await _patientService.SearchByNameAsync(name);

            return Ok(patients);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            CreatePatientDto dto)
        {
            try
            {
                var patient =
                    await _patientService.CreateAsync(dto);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = patient.PatientId },
                    patient);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(
            int id,
            UpdatePatientDto dto)
        {
            try
            {
                if (User.IsInRole("Patient"))
                {
                    var patientIdFromToken = GetPatientReferenceIdFromToken();

                    if (!patientIdFromToken.HasValue ||
                        patientIdFromToken.Value != id)
                    {
                        return Forbid();
                    }
                }

                await _patientService.UpdateAsync(id, dto);

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:int}/activate")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Activate(int id)
        {
            try
            {
                await _patientService.ActivateAsync(id);

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{id:int}/deactivate")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Deactivate(int id)
        {
            try
            {
                await _patientService.DeactivateAsync(id);

                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        private int? GetPatientReferenceIdFromToken()
        {
            var referenceIdValue = User.FindFirst("ReferenceId")?.Value;

            if (int.TryParse(referenceIdValue, out var referenceId))
            {
                return referenceId;
            }

            return null;
        }
    }
}

