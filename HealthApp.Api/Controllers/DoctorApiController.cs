using System.Security.Claims;
using HealthApp.Api.Dto;
using HealthApp.Api.Service.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        // GET all active doctors
        [HttpGet("activedoctors")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllActive()
        {
            var data = await _service.GetAllActiveDoctorAsync();
            return Ok(data);
        }

        // GET all doctors
        [HttpGet("all")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllDoctorsAsync();
            return Ok(data);
        }

        // GET logged-in doctor profile
        [HttpGet("me")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Doctor")]
        public async Task<IActionResult> GetMyProfile()
        {
            var identityUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(identityUserId))
            {
                return Unauthorized("Invalid token.");
            }

            var doctor = await _service.GetMyProfileAsync(identityUserId);
            return Ok(doctor);
        }

        // GET doctor by ID
        [HttpGet("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> GetById(int id)
        {
            var doctor = await _service.GetDoctorByIdAsync(id);
            return Ok(doctor);
        }

        // SEARCH by specialisation
        [HttpGet("specialisation/{type}")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchBySpecialisation(string type)
        {
            var result = await _service.SearchBySpecialisationAsync(type);
            return Ok(result);
        }

        // UPDATE doctor by ID (Admin only)
        [HttpPut("{id:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> UpdateDoctor(int id, [FromBody] DoctorDto dto)
        {
            var result = await _service.UpdateDoctorByIdAsync(id, dto);
            return Ok(result);
        }
    }
}