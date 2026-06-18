using HealthAxis.API.DTOs;
using HealthAxis.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IDoctorService _doctorService;
    private readonly IAppointmentService _appointmentService;

    public AdminController(
        IDoctorService doctorService,
        IAppointmentService appointmentService)
    {
        _doctorService = doctorService;
        _appointmentService = appointmentService;
    }

    // ✅ GET all doctors
    [HttpGet("doctors")]
    public async Task<IActionResult> GetDoctors()
    {
        var result = await _doctorService.GetAllAsync();
        return Ok(result);
    }

    // ✅ ADD doctor (if requirement demands)
    [HttpPost("doctors")]
    public async Task<IActionResult> CreateDoctor(CreateDoctorDto dto)
    {
        var result = await _doctorService.AddAsync(dto); // adjust if needed
        return Ok(result);
    }

    // ✅ UPDATE doctor
    [HttpPut("doctors/{id}")]
    public async Task<IActionResult> UpdateDoctor(int id, UpdateDoctorDto dto)
    {
        var result = await _doctorService.UpdateAsync(id, dto); // adjust
        return Ok(result);
    }

    // ✅ REPORT (basic)
    [HttpGet("reports/appointments")]
    public async Task<IActionResult> GetReports()
    {
        var result = await _appointmentService.GetAllAsync(); // simplified report
        return Ok(result);
    }
}