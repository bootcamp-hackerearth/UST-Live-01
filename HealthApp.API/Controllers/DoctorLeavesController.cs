using HealthApp.API.Service.Interface;
using HealthApp.Shared.Constants;
using HealthApp.Shared.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthApp.API.Controllers;

[ApiController]
[Route("api/doctor-leaves")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class DoctorLeavesController(
    IDoctorLeaveService doctorLeaveService) : ControllerBase
{
    [HttpPost("me/preview")]
    [Authorize(Roles = Roles.Doctor)]
    public async Task<ActionResult<DoctorLeaveImpactDto>>
        PreviewMyLeaveImpact(
            CreateDoctorLeaveDto dto,
            CancellationToken ct)
    {
        var impact =
            await doctorLeaveService.PreviewMyLeaveImpactAsync(
                dto,
                ct);

        return Ok(impact);
    }

    [HttpPost("me")]
    [Authorize(Roles = Roles.Doctor)]
    public async Task<ActionResult<DoctorLeaveCreationResultDto>>
        CreateMyLeave(
            CreateDoctorLeaveDto dto,
            CancellationToken ct)
    {
        var result =
            await doctorLeaveService.CreateMyLeaveAsync(
                dto,
                ct);

        return CreatedAtAction(
            nameof(GetMyLeaveHistory),
            value: result);
    }

    [HttpGet("me")]
    [Authorize(Roles = Roles.Doctor)]
    public async Task<ActionResult<List<DoctorLeaveDto>>>
        GetMyLeaveHistory(
            CancellationToken ct)
    {
        var leaveHistory =
            await doctorLeaveService.GetMyLeaveHistoryAsync(ct);

        return Ok(leaveHistory);
    }

    [HttpGet("doctor/{doctorId:int}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<ActionResult<List<DoctorLeaveDto>>>
        GetDoctorLeaveHistory(
            int doctorId,
            CancellationToken ct)
    {
        var leaveHistory =
            await doctorLeaveService.GetDoctorLeaveHistoryAsync(
                doctorId,
                ct);

        return Ok(leaveHistory);
    }

    [HttpGet("doctor/{doctorId:int}/status")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<ActionResult<DoctorLeaveStatusDto>>
        GetDoctorLeaveStatus(
            int doctorId,
            [FromQuery] DateTime date,
            CancellationToken ct)
    {
        var selectedDate = date == default
            ? DateTime.Today
            : date.Date;

        var leaveStatus =
            await doctorLeaveService.GetDoctorLeaveStatusAsync(
                doctorId,
                selectedDate,
                ct);

        return Ok(leaveStatus);
    }
}