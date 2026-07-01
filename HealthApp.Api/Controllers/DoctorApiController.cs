using HealthApp.Api.Exceptions;
using HealthApp.Api.Service.Interface;
using HealthApp.Shared.Dto;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthApp.Api.Controllers
{
    [Route("api/doctors")]
    [ApiController]
    public class DoctorApiController : ControllerBase
    {
        private readonly IDoctorService _service;

        public DoctorApiController(IDoctorService service)
        {
            _service = service;
        }

        [HttpGet("me")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Doctor")]
        public async Task<IActionResult> GetMyProfile()
        {
            try
            {
                var identityUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrWhiteSpace(identityUserId))
                    return Unauthorized(new { message = "Invalid token." });

                var doctor = await _service.GetMyProfileAsync(identityUserId);
                return Ok(doctor);
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

        [HttpGet("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var doctor = await _service.GetDoctorByIdAsync(id);
                return Ok(doctor);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
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

        [HttpPut("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> UpdateDoctor(int id, [FromBody] DoctorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); 

            try
            {
                var result = await _service.UpdateDoctorByIdAsync(id, dto);
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
            catch (EntityNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch
            {
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpGet("activedoctors")]
        [AllowAnonymous]
        public async Task<IActionResult> GetActiveDoctors(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var (data, total) = await _service.GetPagedActiveDoctorsAsync(pageNumber, pageSize);

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

        [HttpGet("all")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> GetAllDoctors(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var (data, total) = await _service.GetPagedDoctorsAsync(pageNumber, pageSize);

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

        [HttpGet("specialisation/{type}")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchBySpecialisation(
            string type, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var (data, total) =
                    await _service.SearchBySpecialisationPagedAsync(type, pageNumber, pageSize);

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
    }
}