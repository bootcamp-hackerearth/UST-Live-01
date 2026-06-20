using HealthApp.Api.Dtos;
using HealthApp.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthApp.Api.Controllers
{
    [ApiController]
    [Route("api/admin/doctors")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public class AdminDoctorsController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public AdminDoctorsController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDoctors()
        {
            var doctors = await _doctorService.GetAllDoctorsAsync();

            return Ok(doctors);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateDoctor(
            int id,
            [FromBody] DoctorCreateDto dto)
        {
            await _doctorService.UpdateDoctorAsync(id, dto);

            return Ok(new
            {
                message = "Doctor updated successfully."
            });
        }

        [HttpPatch("{id:int}/status")]
        public async Task<IActionResult> ChangeDoctorStatus(
            int id,
            [FromQuery] bool isActive)
        {
            await _doctorService.ChangeStatusAsync(id, isActive);

            return Ok(new
            {
                message = "Doctor status updated successfully."
            });
        }
    }
}