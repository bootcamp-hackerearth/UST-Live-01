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
    public async Task<ActionResult<List<DoctorDto>>> Doctors()
        => Ok(await adminService.GetDoctorsAsync());

    [HttpPost("doctors")]
    public async Task<ActionResult<DoctorDto>> CreateDoctor(CreateDoctorDto dto)
        => Ok(await adminService.CreateDoctorAsync(dto));

    [HttpPut("doctors/{id:int}")]
    public async Task<ActionResult<DoctorDto>> UpdateDoctor(int id, UpdateDoctorDto dto)
        => Ok(await adminService.UpdateDoctorAsync(id, dto));

    [HttpGet("reports/appointments")]
    public async Task<ActionResult<List<AppointmentReportDto>>> Reports()
        => Ok(await adminService.GetAppointmentReportsAsync());

    [HttpGet("users")]
    public async Task<ActionResult<List<UserDto>>> GetUsers([FromQuery] string? role)
            => Ok(await adminService.GetUsersAsync(role));

    [HttpGet("patients")]
    public async Task<ActionResult<List<PatientDto>>> GetPatients(
    [FromQuery] string? search,
    [FromQuery] GenderType? gender,
    [FromQuery] bool? hasInsurance)
    => Ok(await adminService.GetPatientsAsync(search, gender, hasInsurance));

}