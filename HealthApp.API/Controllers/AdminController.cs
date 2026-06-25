using HealthApp.API.Service.Interface;
using HealthApp.Shared.DTOs;
using HealthApp.Shared.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthApp.API.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
public class AdminController(IAdminService adminService) : ControllerBase
{
    [HttpGet("doctors")]
    public async Task<ActionResult<PagedResultDto<DoctorDto>>> Doctors(
    [FromQuery] PaginationQueryDto pagination)
    => Ok(await adminService.GetDoctorsAsync(pagination));

    [HttpPost("doctors")]
    public async Task<ActionResult<CreateDoctorResponseDto>> CreateDoctor(CreateDoctorDto dto)
        => Ok(await adminService.CreateDoctorAsync(dto));

    [HttpPut("doctors/{id:int}")]
    public async Task<ActionResult<DoctorDto>> UpdateDoctor(int id, UpdateDoctorDto dto)
        => Ok(await adminService.UpdateDoctorAsync(id, dto));

    [HttpGet("reports/appointments")]
    public async Task<ActionResult<PagedResultDto<AppointmentReportDto>>> Reports(
    [FromQuery] PaginationQueryDto pagination)
    => Ok(await adminService.GetAppointmentReportsAsync(pagination));

    [HttpGet("users")]
    public async Task<ActionResult<List<UserDto>>> GetUsers([FromQuery] string? role)
            => Ok(await adminService.GetUsersAsync(role));

    [HttpGet("patients")]
    public async Task<ActionResult<PagedResultDto<PatientDto>>> GetPatients(
    [FromQuery] string? search,
    [FromQuery] GenderType? gender,
    [FromQuery] bool? hasInsurance,
    [FromQuery] PaginationQueryDto pagination)
    => Ok(await adminService.GetPatientsAsync(
        search,
        gender,
        hasInsurance,
        pagination));

}