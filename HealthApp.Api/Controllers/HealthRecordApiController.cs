using HealthApp.Api.Service.Interface;
using HealthApp.Shared.Dto;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthRecordApiController : ControllerBase
    {

        private readonly IHealthRecordService _service;

        public HealthRecordApiController(IHealthRecordService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Doctor")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllRecordsAsync();
            return Ok(data);
        }

        [HttpGet]
        [Route("filter")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Doctor,Admin")]
        public async Task<IActionResult> GetByDoctorAndPatient([FromQuery] int doctorId, [FromQuery] int patientId)
        {
            var data = await _service.GetHealthRecordsByDoctorAsync(doctorId, patientId);
            return Ok(data);
        }

        [HttpPost]
        [Route("")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Doctor")]
        public async Task<IActionResult> Create(HealthRecordDto dto)
        {
                await _service.AddRecordAsync(dto);

            return Ok(new
            {
                success = true,
                message = "Health record created successfully."
            });


        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Doctor,Admin")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _service.GetRecordByIdAsync(id);
            return Ok(data);
        }




        [HttpGet("me")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        public async Task<IActionResult> GetMyRecords()
        {
            var identityUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(identityUserId))
                return Unauthorized(new { message = "Invalid token." });

            var data = await _service.GetRecordsByUserAsync(identityUserId);

            return Ok(data);
        }


        [HttpGet("doctor/me")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Doctor")]
        public async Task<IActionResult> GetDoctorRecords()
        {
            var identityUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(identityUserId))
                return Unauthorized(new { message = "Invalid token." });

            var data = await _service.GetRecordsByDoctorAsync(identityUserId);

            return Ok(data);
        }


    }
}
