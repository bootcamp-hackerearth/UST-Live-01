using Healthcare.Shared.DTOs.Authentication;
using Healthcare.Shared.DTOs.Doctor;
using HealthCare.Api.Services.Implementations;
using HealthCare.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthCare.Api.Controllers
{
    [Route("api/admin")]
    [ApiController]
    [Authorize]
    public class AdminDoctorController : ControllerBase
    {

        private readonly IDoctorService _doctorService;


        public AdminDoctorController(IDoctorService doctorService)
        {

            _doctorService = doctorService;

        }

        [HttpGet("doctors/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> GetDoctorById(int id)
        {
            var result = await _doctorService.GetByIdAsync(id);
            if (result == null)
                throw new Exception("Doctor Not Found");

            return Ok(result);
        }
        [HttpGet("doctors")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> GetAllDoctor([FromQuery] DoctorFilter filter)
        {
            var result = await _doctorService.GetAllAsync(filter);
            return Ok(result);
        }


        [HttpPut("doctors/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> UpdateDoctor(int id, [FromBody] UpdateDoctorDto dto)
        {

            await _doctorService.UpdateAsync(id, dto);
            return Ok();
        }

        [HttpPatch("doctors/{id}/status")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> UpdateDoctorStatus(int id, [FromBody] bool isActive)
        {
            await _doctorService.UpdateStatusAsync(id, isActive);
            return Ok();
        }

        [HttpDelete("doctors/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            await _doctorService.DeleteAsync(id);
            return Ok();
        }
    }
}
