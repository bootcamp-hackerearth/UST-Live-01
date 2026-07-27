using HealthAxisApplicn.Dto.Patients;
using HealthAxisApplicn.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthAxisApplicn.Controllers
{
    [Route("api/patients")]
    [ApiController]
    [Authorize]
    public class PatientController(IPatientService service) : ControllerBase
    {
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var result = await service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var result = await service.GetByIdAsync(id);
            if (result is null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] CreatePatientDto entity)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await service.CreateAsync(entity);
            if (result is null) return NotFound();
            return CreatedAtAction("GetById", new {id = result.PatientId}, result);
        }


        [HttpPut("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePatientDto entity)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await service.UpdateAsync(id, entity);
            if (result is null) return NotFound();
            return Ok(result);
        }

        [HttpPut("toggle/{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> Toggle(int id)
        {
            var result = await service.DeactivatePatientAsync(id);
            return Ok(result);
        }

        [HttpGet("name/{name}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> GetByPatientName(string name)
        {
            var result = await service.SearchByPatientNameAsync(name);
            if (result.Count == 0) return NotFound();
            return Ok(result);
        }

        [HttpGet("search")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> Search(
        [FromQuery] string? name,
        [FromQuery] string? phone)
        {
        var result = await service.SearchAsync(name, phone);
        return Ok(result);
        }

        [HttpGet("me")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> GetCurrentPatient()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var patient = await service.GetByUserIdAsync(userId);

            if (patient == null)
                return NotFound();

            return Ok(new
            {
                patientName = patient.PatientName
            });
        }

        [HttpGet("profile")]
        [Authorize(Roles = "Patient")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirst(
                System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var patient = await service.GetByUserIdAsync(userId);

            if (patient == null)
                return NotFound();

            return Ok(patient);
        }

        [HttpGet("doctor/patient/{patientId:int}")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> GetPatientDetails(int patientId)
        {
            var doctorId =
                int.Parse(User.FindFirst("DoctorId")!.Value);

            var result =
                await service.GetPatientDetailsForDoctorAsync(
                    doctorId,
                    patientId);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

    }
}
