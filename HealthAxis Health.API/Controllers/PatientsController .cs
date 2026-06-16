using HealthAxisHealth.Shared.DTOs.PatientDtos;
using HealthAxisHealth.API.Helpers;
using HealthAxisHealth.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthAxisHealth.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PatientsController :
        ControllerBase
    {
        #region Fields

        private readonly IPatientService
            _patientService;

        #endregion

        #region Constructor

        public PatientsController(
            IPatientService patientService)
        {
            _patientService = patientService;
        }

        #endregion

        #region Properties

        private int CurrentUserId =>
            int.Parse(
                User.FindFirst(
                    ClaimTypes.NameIdentifier)!
                .Value);

        #endregion

        #region Admin Endpoints

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult>
            GetPatients(
                [FromQuery]
                PaginationParams pagination)
        {
            var patients =
                await _patientService
                    .GetPagedAsync(
                        pagination);

            return Ok(patients);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult>
            GetPatient(
                int id)
        {
            var patient =
                await _patientService
                    .GetByIdAsync(id);

            return Ok(patient);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult>
            UpdatePatient(
                int id,
                UpdatePatientDto dto)
        {
            await _patientService
                .UpdateAsync(
                    id,
                    dto);

            return Ok(new
            {
                Message =
                    "Patient updated successfully."
            });
        }

        [HttpGet("{id:int}/health-records")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult>
            GetPatientHealthRecords(
                int id)
        {
            var records =
                await _patientService
                    .GetHealthRecordsAsync(id);

            return Ok(records);
        }

        #endregion

        #region Patient Endpoints

        [HttpGet("profile")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult>
            GetProfile()
        {
            var patient =
                await _patientService
                    .GetByUserIdAsync(
                        CurrentUserId);

            return Ok(patient);
        }

        [HttpPut("profile")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult>
            UpdateProfile(
                UpdatePatientDto dto)
        {
            await _patientService
                .UpdateByUserIdAsync(
                    CurrentUserId,
                    dto);

            return Ok(new
            {
                Message =
                    "Patient profile updated successfully."
            });
        }

        [HttpGet("profile/health-records")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult>
            GetMyHealthRecords()
        {
            var records =
                await _patientService
                    .GetHealthRecordsByUserIdAsync(
                        CurrentUserId);

            return Ok(records);
        }

        #endregion
    }
}
