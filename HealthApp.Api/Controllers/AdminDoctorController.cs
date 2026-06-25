using HealthApp.Api.Services.Interfaces;
using HealthApp.Shared.Dtos;
using HealthApp.Shared.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthApp.Api.Controllers
{
    [ApiController]
    [Route("api/admin/doctors")]
    [Authorize(
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
        Roles = "Admin")]
    public class AdminDoctorsController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public AdminDoctorsController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDoctors(
            [FromQuery] string? search,
            [FromQuery] SpecialisationType? specialisation,
            [FromQuery] bool? isActive,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var doctors = await _doctorService.SearchDoctorsAsync(
                search,
                specialisation,
                isActive,
                pageNumber,
                pageSize);

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