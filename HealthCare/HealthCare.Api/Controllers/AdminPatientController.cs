using Healthcare.Shared.DTOs.Authentication;
using Healthcare.Shared.DTOs.Patient;
using HealthCare.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace HealthCare.Api.Controllers
{

    [ApiController]
    [Route("api/admin/patients")]
    public class AdminPatientController : ControllerBase
    {
        private readonly IPatientService _patientService;
        private readonly IAuthService _authService;
        public AdminPatientController(IPatientService patientService, IAuthService authService)
        {
            _patientService = patientService;
            _authService = authService;
        }

        // Get patient by id

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> GetPatientById(int id)
        {

            if (id <= 0)
            {
                return BadRequest("Invalid patient id.");
            }

            var result = await _patientService.GetByIdAsync(id);
            return Ok(result);
        }

        //Get All pat
        [HttpGet()]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> GetAllPatient([FromQuery] PatientFilter filter)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _patientService.GetAllAsync(filter);
            return Ok(result);
        }

        //update patient
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> UpdatePatient(int id, [FromBody] UpdatePatientDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id <= 0)
                return BadRequest("Invalid patient id.");

            await _patientService.UpdateAsync(id, dto);
            return Ok();
        }

        //Update status
        [HttpPatch("{id}/status")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> UpdatePatientStatus(int id, [FromBody] bool isActive)
        {

            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            await _patientService.UpdateStatusAsync(id, isActive);
            return Ok();
        }

        //delete patient
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> DeletePatient(int id)
        {

            if (id <= 0)
            {
                return BadRequest("Invalid patient id.");
            }

            await _patientService.DeleteAsync(id);
            return Ok();
        }

        //register
        [HttpPost("register")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> RegisterPatient([FromBody] CreatePatientDto dto)
        {

            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            await _authService.RegisterPatientAsync(dto);
            return Ok(new { message = "Registration successful" });
        }

        //Serach by name 
        [HttpGet("search")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> SearchByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Name is required");

            var result = await _patientService.SearchByNameAsync(name);

            if (!result.Any())
                return NotFound($"No patients found with name '{name}'");

            return Ok(result);
        }
    }

}
