using HealthApp.Api.Dto;
using HealthApp.Api.Service.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientApiController : ControllerBase
    {
        private readonly IPatientService _service;

        public PatientApiController(IPatientService service)
        {
            _service = service;
        }

        // GET all patients
        [HttpGet]
        [Route("")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> Get()
        {
            var data = await _service.GetAllPatientsAsync();
            return Ok(data);
        }

        // GET patient by ID
        [HttpGet]
        [Route("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> GetById(int id)
        {
            var patient = await _service.GetPatientByIdAsync(id);
            return Ok(patient);
        }

        // GET logged-in user's patient profile
        [HttpGet("me")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        public async Task<IActionResult> GetMyProfile()
        {
            var identityUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(identityUserId))
            {
                return Unauthorized("Invalid token.");
            }

            var patient = await _service.GetMyProfileAsync(identityUserId);
            return Ok(patient);
        }

        // UPDATE logged-in user's patient profile
        [HttpPut("me")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        public async Task<IActionResult> UpdateMyProfile([FromBody] PatientDto dto)
        {
            var identityUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(identityUserId))
            {
                return Unauthorized("Invalid token.");
            }

            await _service.UpdateMyProfileAsync(identityUserId, dto);
            return Ok("Patient updated successfully");
        }
    }
}