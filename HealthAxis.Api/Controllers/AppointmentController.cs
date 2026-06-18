using HealthAxisCore_Api.Models.Dtos;
using HealthAxisCore_Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace HealthAxisCore_Api.Controllers
{
    [ApiController]
    [Route("api/appointments")]
    [Authorize]
    public class AppointmentController(IAppointmentService service) : ControllerBase
    {
        [HttpGet] public async Task<ActionResult<List<AppointmentDto>>> GetAppointments([FromQuery] int? patientId, [FromQuery] int? doctorId, [FromQuery] DateTime? date, CancellationToken ct) => Ok(await service.GetAppointmentsAsync(patientId, doctorId, date, User, ct));
        [HttpPost] [Authorize(Roles="Patient")] public async Task<ActionResult<AppointmentDto>> Create(CreateAppointmentDto request, CancellationToken ct) => Ok(await service.CreateAsync(request, User, ct));
        [HttpPut("{id:int}/status")] public async Task<ActionResult<AppointmentDto>> UpdateStatus(int id, UpdateAppointmentStatusDto request, CancellationToken ct) => Ok(await service.UpdateStatusAsync(id, request, User, ct));
        [HttpDelete("{id:int}")] [Authorize(Roles="Admin")] public async Task<IActionResult> Delete(int id, CancellationToken ct) { await service.DeleteAsync(id, ct); return NoContent(); }
    }
}
