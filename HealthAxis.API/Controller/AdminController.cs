using HealthAxis.API.Services.Interfaces;
using HealthAxis.Shared.DTO.AdminDtos;
using HealthAxis.Shared.DTO.CommonDtos;
using HealthAxis.Shared.DTO.DoctorDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthAxis.API.Controller
{
    [Route("api/admin")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("doctors")]
        public async Task<IActionResult> AddDoctor(
            [FromBody] CreateDoctorDto doctorDto)
        {
            var doctor = await _adminService.AddDoctorAsync(doctorDto);

            return Ok(doctor);
        }

        [HttpGet("doctors")]
        public async Task<IActionResult> GetAllDoctors()
        {
            var doctors = await _adminService.GetAllDoctorsAsync();

            return Ok(doctors);
        }

        [HttpGet("doctors/paged")]
        public async Task<IActionResult> GetDoctorsPaged(
            [FromQuery] PaginationQueryDto paginationQuery)
        {
            var doctors = await _adminService.GetDoctorsPagedAsync(
                paginationQuery);

            return Ok(doctors);
        }

        [HttpPut("doctors/{id}")]
        public async Task<IActionResult> UpdateDoctor(
            int id,
            [FromBody] UpdateDoctorDto doctorDto)
        {
            var doctor = await _adminService.UpdateDoctorAsync(id, doctorDto);

            return Ok(doctor);
        }

        [HttpPut("appointments/{id}/status")]
        public async Task<IActionResult> UpdateAppointmentStatus(
    int id,
    [FromBody] AdminUpdateAppointmentStatusDto statusDto)
        {
            var appointment = await _adminService.UpdateAppointmentStatusByAdminAsync(
                id,
                statusDto);

            return Ok(appointment);
        }

        [HttpGet("reports/appointments")]
        public async Task<IActionResult> GetAppointmentReports()
        {
            var reports = await _adminService.GetAppointmentReportsAsync();

            return Ok(reports);
        }

        [HttpGet("reports/appointments/details")]
        public async Task<IActionResult> GetAppointmentDetails()
        {
            var reports = await _adminService.GetAppointmentDetailsAsync();

            return Ok(reports);
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _adminService.GetUsersAsync();

            return Ok(users);
        }
        [HttpGet("patients")]
        public async Task<IActionResult> GetPatients()
        {
            var patients = await _adminService.GetPatientsAsync();

            return Ok(patients);
        }

        [HttpPut("patients/{id}")]
        public async Task<IActionResult> UpdatePatient(
            int id,
            [FromBody] UpdateAdminPatientDto patientDto)
        {
            var patient = await _adminService.UpdatePatientAsync(id, patientDto);

            return Ok(patient);
        }

        [HttpGet("patients/{id}/appointments")]
        public async Task<IActionResult> GetPatientAppointments(int id)
        {
            var appointments = await _adminService.GetPatientAppointmentsAsync(id);

            return Ok(appointments);
        }

        [HttpGet("users/paged")]
        public async Task<IActionResult> GetUsersPaged(
            [FromQuery] AdminUserQueryDto queryDto)
        {
            var users = await _adminService.GetUsersPagedAsync(queryDto);

            return Ok(users);
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var profile = await _adminService.GetAdminProfileAsync(
                GetCurrentUserId());

            return Ok(profile);
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile(
            [FromBody] UpdateAdminProfileDto profileDto)
        {
            var profile = await _adminService.UpdateAdminProfileAsync(
                GetCurrentUserId(),
                profileDto);

            return Ok(profile);
        }

        [HttpPut("profile/change-password")]
        public async Task<IActionResult> ChangePassword(
            [FromBody] ChangePasswordDto passwordDto)
        {
            await _adminService.ChangeAdminPasswordAsync(
                GetCurrentUserId(),
                passwordDto);

            return Ok(new
            {
                message = "Password changed successfully."
            });
        }

        private string GetCurrentUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ??
                throw new UnauthorizedAccessException("User id not found.");
        }
    }
}