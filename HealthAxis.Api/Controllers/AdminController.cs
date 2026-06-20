using HealthAxisCore_Api.Models.Dtos;
using HealthAxisCore_Api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace HealthAxisCore_Api.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public class AdminController(IAdminService service) : ControllerBase
    {
        [HttpGet("doctors")]
        public async Task<ActionResult<List<DoctorDto>>> GetDoctors(CancellationToken ct
            ) => Ok(await service.GetDoctorsAsync(ct));
        [HttpPost("doctors")]
        public async Task<ActionResult<DoctorDto>> CreateDoctor(CreateDoctorDto request, CancellationToken ct
            ) => Ok(await service.CreateDoctorAsync(request, ct));
        [HttpPut("doctors/{id:int}")]
        public async Task<ActionResult<DoctorDto>> UpdateDoctor(int id, UpdateDoctorDto request, CancellationToken ct
            ) => Ok(await service.UpdateDoctorAsync(id, request, ct));
        [HttpGet("users")]
        public async Task<ActionResult<List<UserDto>>> GetUsers([FromQuery] string? role
            ) => Ok(await service.GetUsersAsync(role));
        [HttpGet("reports/appointments")]
        public async Task<ActionResult<List<AppointmentReportDto>>> GetAppointmentReport(CancellationToken ct
            ) => Ok(await service.GetAppointmentReportAsync(ct));
        [HttpPut("patients/{patientId:int}/status")]
        public async Task<IActionResult> UpdatePatientStatus(int patientId, [FromQuery] bool isActive, CancellationToken ct)
        {
            await service.UpdatePatientStatusAsync(patientId, isActive, ct); return NoContent();
        }
    }
}
