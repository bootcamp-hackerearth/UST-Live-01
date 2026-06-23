
using HealthApp.API.Service.Interface;
using HealthApp.Shared.Constants;
using HealthApp.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthApp.API.Controllers;

[ApiController]
[Route("api/appointments")]
[Authorize]
public class AppointmentsController(IAppointmentService appointmentService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<AppointmentDto>>> Get(
        [FromQuery] int? patientId,
        [FromQuery] int? doctorId)
    {
        if (patientId.HasValue)
            return Ok(await appointmentService.GetAppointmentsByPatientIdAsync(patientId.Value));

        if (doctorId.HasValue)
            return Ok(await appointmentService.GetAppointmentsByDoctorIdAsync(doctorId.Value));

        return Ok(await appointmentService.GetAllAppointmentsAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AppointmentDto>> GetById(int id)
        => Ok(await appointmentService.GetAppointmentByIdAsync(id));

    [HttpPost]
    [Authorize(Roles = Roles.Patient + "," + Roles.Admin)]
    public async Task<ActionResult<AppointmentDto>> Post(BookAppointmentDto dto)
        => Ok(await appointmentService.BookAppointmentAsync(dto));

    [HttpPut("{id:int}/status")]
    public async Task<ActionResult<AppointmentDto>> Status(int id, UpdateAppointmentStatusDto dto)
        => Ok(await appointmentService.ChangeAppointmentStatusAsync(id, dto));

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<AppointmentDto>> Delete(int id, [FromQuery] string? reason)
        => Ok(await appointmentService.CancelAppointmentAsync(id, reason));
}