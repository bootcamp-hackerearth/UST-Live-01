using HealthApp.Shared.Dto;
using HealthApp.Api.Service.Interface;
using HealthApp.Api.Exceptions;
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


        [HttpGet("paged")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> GetPaged(int pageNumber = 1, int pageSize = 10, string? search = null)
        {
            try
            {
                var (data, total) =
                    await _service.GetPagedPatientsAsync(pageNumber, pageSize, search);

                return Ok(new
                {
                    data,
                    totalRecords = total,
                    pageNumber,
                    pageSize
                });
            }
            catch (BusinessRuleException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Doctor,Admin")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var patient = await _service.GetPatientByIdAsync(id);
                return Ok(patient);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (BusinessRuleException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] PatientDto dto)
        {
            try
            {
                var result = await _service.AddPatientAsync(dto);
                return Ok(result);
            }
            catch (ConflictException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (BusinessRuleException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch
            {
                return StatusCode(500, new { message = "Internal server error" });
            }
        }


        [HttpGet("me")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        public async Task<IActionResult> GetMyProfile()
        {
            try
            {
                var identityUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrWhiteSpace(identityUserId))
                    return Unauthorized(new { message = "Invalid token." });

                var patient = await _service.GetMyProfileAsync(identityUserId);
                return Ok(patient);
            }
            catch (BusinessRuleException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("me")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        public async Task<IActionResult> UpdateMyProfile([FromBody] PatientDto dto)
        {
            try
            {
                var identityUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrWhiteSpace(identityUserId))
                    return Unauthorized(new { message = "Invalid token." });

                var updated = await _service.UpdateMyProfileAsync(identityUserId, dto);

                return Ok(updated);
            }
            catch (ConflictException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (BusinessRuleException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch
            {
                return StatusCode(500, new { message = "Internal server error" });
            }
        }
    }
}