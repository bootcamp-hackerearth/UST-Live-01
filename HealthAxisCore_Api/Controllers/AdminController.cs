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
        public async Task<ActionResult<PagedResultDto<DoctorDto>>> GetDoctors(
            [FromQuery] PaginationQueryDto query,
            CancellationToken ct)
        {
            var result = await service.GetDoctorsAsync(query, ct);

            return Ok(result);
        }

        [HttpPost("doctors")]
        public async Task<ActionResult<DoctorDto>> CreateDoctor(
            CreateDoctorDto request,
            CancellationToken ct)
        {
            var result = await service.CreateDoctorAsync(request, ct);

            return Ok(result);
        }

        [HttpPut("doctors/{id:int}")]
        public async Task<ActionResult<DoctorDto>> UpdateDoctor(
            int id,
            UpdateDoctorDto request,
            CancellationToken ct)
        {
            var result = await service.UpdateDoctorAsync(id, request, ct);

            return Ok(result);
        }

        [HttpPut("doctors/{doctorId:int}/status")]
        public async Task<IActionResult> UpdateDoctorStatus(
            int doctorId,
            [FromQuery] bool isActive,
            CancellationToken ct)
        {
            await service.UpdateDoctorStatusAsync(
                doctorId,
                isActive,
                ct);

            return NoContent();
        }

        [HttpGet("users")]
        public async Task<ActionResult<PagedResultDto<UserDto>>> GetUsers(
            [FromQuery] string? role,
            [FromQuery] PaginationQueryDto query,
            CancellationToken ct)
        {
            var result = await service.GetUsersAsync(
                role,
                query,
                ct);

            return Ok(result);
        }

        [HttpGet("reports/appointments")]
        public async Task<ActionResult<List<AppointmentReportDto>>> GetAppointmentReport(
            CancellationToken ct)
        {
            var result = await service.GetAppointmentReportAsync(ct);

            return Ok(result);
        }

        [HttpPut("patients/{patientId:int}/status")]
        public async Task<IActionResult> UpdatePatientStatus(
            int patientId,
            [FromQuery] bool isActive,
            CancellationToken ct)
        {
            await service.UpdatePatientStatusAsync(
                patientId,
                isActive,
                ct);

            return NoContent();
        }
    }
}