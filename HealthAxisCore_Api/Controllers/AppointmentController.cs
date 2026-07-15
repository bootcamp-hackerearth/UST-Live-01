using HealthAxisCore_Api.Models.Dtos;
using HealthAxisCore_Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxisCore_Api.Controllers
{
    [ApiController]
    [Route("api/appointments")]
    [Authorize(Roles = "Patient,Doctor,Admin")]
    public class AppointmentController(IAppointmentService service) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<AppointmentDto>>> GetAppointments(
            [FromQuery] int? patientId,
            [FromQuery] int? doctorId,
            [FromQuery] DateTime? date,
            CancellationToken ct)
        {
            return Ok(await service.GetAppointmentsAsync(
                patientId,
                doctorId,
                date,
                User,
                ct));
        }

        [HttpPost]
        [Authorize(Roles = "Patient")]
        public async Task<ActionResult<AppointmentDto>> Create(
    CreateAppointmentDto request,
    CancellationToken ct)
        {
            var createdAppointment = await service.CreateAsync(request, User, ct);

            return StatusCode(StatusCodes.Status201Created, createdAppointment);
        }

        [HttpPut("{id:int}/status")]
        [Authorize(Roles = "Patient,Doctor,Admin")]
        public async Task<ActionResult<AppointmentDto>> UpdateStatus(
            int id,
            UpdateAppointmentStatusDto request,
            CancellationToken ct)
        {
            return Ok(await service.UpdateStatusAsync(id, request, User, ct));
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(
            int id,
            CancellationToken ct)
        {
            await service.DeleteAsync(id, ct);

            return NoContent();
        }
    }
}