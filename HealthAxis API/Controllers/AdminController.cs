using HealthAxis.API.DTOs.Doctors;
using HealthAxis.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxis.API.Controllers
{
    [Route("api/admin")]
    [ApiController]
    [Authorize(Roles = "Admin")]
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

        [HttpGet("doctors")]
        public async Task<IActionResult> GetDoctors(CancellationToken ct)
        {
            var doctors =
                await _doctorService.GetAllAsync(ct);

            return Ok(doctors);
        }

        [HttpPost("doctors")]
        public async Task<IActionResult> CreateDoctor(
            DoctorCreateDto request,
            CancellationToken ct)
        {
            var doctor =
                await _doctorService.CreateAsync(request, ct);

            return Ok(doctor);
        }

        [HttpPut("doctors/{id:int}")]
        public async Task<IActionResult> UpdateDoctor(
            int id,
            DoctorUpdateDto request,
            CancellationToken ct)
        {
            var doctor =
                await _doctorService.UpdateAsync(id, request, ct);

            if (doctor == null)
            {
                return NotFound(new { message = "Doctor not found." });
            }

            return Ok(doctor);
        }

        [HttpGet("reports/appointments")]
        public async Task<IActionResult> GetAppointmentReport(
            CancellationToken ct)
        {
            var report =
                await _appointmentService.GetAppointmentReportAsync(ct);

            return Ok(report);
        }
    }
}
