using HealthCareApp.Shared.Dtos;
using HealthCareApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HealthCareApp.Shared.Dtos.Pagination;
using HealthCareApp.Shared.Dtos.Doctors;


namespace HealthCareApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
        Roles = "Admin")]
    public class AdminController(IDoctorService doctorService) : ControllerBase
    {
        [HttpPost("doctors")]
        public async Task<IActionResult> CreateDoctor([FromBody] CreateDoctorDto request)
        {
            var result = await doctorService.CreateDoctorByAdminAsync(request);

            return Ok(result);
        }

        [HttpGet("doctors")]
        public async Task<IActionResult> GetAllDoctors([FromQuery] DoctorPaginationQueryDto query)
        {
            var result = await doctorService.GetAllDoctorsPagedAsync(query);

            return Ok(result);
        }

        [HttpPut("doctors/{doctorId:int}")]
        public async Task<IActionResult> UpdateDoctor(
            [FromRoute] int doctorId,
            [FromBody] UpdateDoctorDto request)
        {
            var result = await doctorService.UpdateDoctorAsync(doctorId, request);

            return Ok(result);
        }

        [HttpDelete("doctors/{doctorId:int}")]
        public async Task<IActionResult> DeleteDoctor([FromRoute] int doctorId)
        {
            var result = await doctorService.DeleteDoctorAsync(doctorId);

            return Ok(result);
        }
    }
}