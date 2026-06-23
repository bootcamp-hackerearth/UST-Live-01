using HealthAxis.API.DTOs.Admin;
using HealthAxis.API.DTOs.CommonDtos;
using HealthAxis.API.DTOs.Doctors;
using HealthAxis.API.DTOs.HealthRecords;
using HealthAxis.API.DTOs.Patients;
using HealthAxis.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthAxis.API.Controllers
{
    [Route("api/admin")]
    [ApiController]
    [Authorize(
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
        Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(
            IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("patients")]
        public async Task<IActionResult> GetPatients(
            [FromQuery] PaginationQueryDto pagination,
            CancellationToken ct)
        {
            var patients =
                await _adminService.GetPatientsAsync(
                    pagination,
                    ct);

            return Ok(patients);
        }

        [HttpPut("patients/{id:int}")]
        public async Task<IActionResult> UpdatePatient(
            int id,
            PatientUpdateDto request,
            CancellationToken ct)
        {
            var patient =
                await _adminService.UpdatePatientAsync(
                    id,
                    request,
                    ct);

            return Ok(patient);
        }

        [HttpGet("doctors")]
        public async Task<IActionResult> GetDoctors(
            [FromQuery] PaginationQueryDto pagination,
            CancellationToken ct)
        {
            var doctors =
                await _adminService.GetDoctorsAsync(
                    pagination,
                    ct);

            return Ok(doctors);
        }

        [HttpPost("doctors")]
        public async Task<IActionResult> CreateDoctor(
            AdminDoctorCreateDto request,
            CancellationToken ct)
        {
            var doctor =
                await _adminService.CreateDoctorAsync(
                    request,
                    ct);

            return Ok(doctor);
        }

        [HttpPut("doctors/{id:int}")]
        public async Task<IActionResult> UpdateDoctor(
            int id,
            DoctorUpdateDto request,
            CancellationToken ct)
        {
            var doctor =
                await _adminService.UpdateDoctorAsync(
                    id,
                    request,
                    ct);

            return Ok(doctor);
        }

        [HttpPut("health-records/{id:int}")]
        public async Task<IActionResult> UpdateHealthRecord(
            int id,
            HealthRecordUpdateDto request,
            CancellationToken ct)
        {
            var healthRecord =
                await _adminService.UpdateHealthRecordAsync(
                    id,
                    request,
                    ct);

            return Ok(healthRecord);
        }

        [HttpGet("reports/appointments")]
        public async Task<IActionResult> GetAppointmentReport(
            [FromQuery] PaginationQueryDto pagination,
            CancellationToken ct)
        {
            var report =
                await _adminService.GetAppointmentReportAsync(
                    pagination,
                    ct);

            return Ok(report);
        }
    }
}
