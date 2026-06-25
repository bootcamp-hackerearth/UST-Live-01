using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HealthApp.Api.Services.Interfaces;
using HealthApp.Shared.Dtos;

namespace HealthApp.Api.Controllers
{
    [ApiController]
    [Route("api/patients")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin")]
        public async Task<IActionResult> GetAllPatients(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var patients = await _patientService.SearchPatientsAsync(
                name: null,
                email: null,
                pageNumber,
                pageSize);

            return Ok(patients);
        }

        [HttpGet("{id:int}")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Doctor,Admin")]
        public async Task<IActionResult> GetPatientById(int id)
        {
            var patient = await _patientService.GetPatientByIdAsync(id);

            return Ok(patient);
        }

        [HttpPut("{id:int}")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Admin")]
        public async Task<IActionResult> UpdatePatient(
            int id,
            [FromBody] PatientCreateDto dto)
        {
            await _patientService.UpdatePatientAsync(id, dto);

            return Ok(new
            {
                message = "Patient updated successfully."
            });
        }

        [HttpGet("search")]
        [Authorize(
            AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
            Roles = "Doctor,Admin")]
        public async Task<IActionResult> SearchPatients(
            [FromQuery] string? name,
            [FromQuery] string? email,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var patients = await _patientService.SearchPatientsAsync(
                name,
                email,
                pageNumber,
                pageSize);

            return Ok(patients);
        }
    }
}